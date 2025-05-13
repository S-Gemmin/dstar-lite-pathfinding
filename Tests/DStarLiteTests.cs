using NUnit.Framework;
using System.Collections.Generic;
public class DStarLiteTests
{
    private Grid grid;
    private DStarLite dStarLite;
    private Vertex v_start;
    private Vertex v_goal;
    private int width = 5;
    private int height = 5;

    [SetUp]
    public void SetUp()
    {
        grid = new Grid(width, height);
        dStarLite = new DStarLite(grid);

        v_start = grid.GetVertex(0, 0);
        v_goal = grid.GetVertex(width - 1, height - 1);
    }

    private List<Vertex> GetPath(Vertex start, Vertex goal)
    {
        var path = new List<Vertex>();
        Vertex current = start;

        while (current != goal && current != null)
        {
            path.Add(current);
            current = dStarLite.FindNext(current);
        }

        if (current == goal) path.Add(goal);
        return path;
    }

    [Test]
    public void FindPath_UnwalkableStart_DoesNotCompute()
    {
        v_start.SetIsWalkable(false);
        dStarLite.FindPath(v_start, v_goal);

        Assert.AreEqual(null, dStarLite.FindNext(v_start));
    }

    [Test]
    public void FindPath_UnwalkableGoal_DoesNotCompute()
    {
        v_goal.SetIsWalkable(false);
        dStarLite.FindPath(v_start, v_goal);

        Assert.AreEqual(null, dStarLite.FindNext(v_start));
    }

    [Test]
    public void FindPath_StartEqualsGoal_DoesNotCompute()
    {
        dStarLite.FindPath(v_start, v_start);

        Assert.AreEqual(null, dStarLite.FindNext(v_start));
    }

    [Test]
    public void FindNext_InitialPath_FirstStepDiagonalCost42()
    {
        dStarLite.FindPath(v_start, v_goal);

        Assert.AreEqual(grid.GetVertex(1, 1), dStarLite.FindNext(v_start));
        Assert.AreEqual(3 * 14, grid.GetVertex(1, 1).gCost);
    }

    [Test]
    public void FindNext_ReturnsNeighborWithLowestGCost()
    {
        v_start.SetGCost(0);
        grid.GetVertex(1, 0).SetGCost(1);
        grid.GetVertex(0, 1).SetGCost(2);
        grid.GetVertex(1, 1).SetGCost(3);

        Vertex nextVertex = dStarLite.FindNext(v_start);

        Assert.AreEqual(grid.GetVertex(1, 0), nextVertex);
    }

    [Test]
    public void FindNext_NoPath_ReturnsNull()
    {
        grid.GetVertex(1, 0).SetIsWalkable(false);
        grid.GetVertex(0, 1).SetIsWalkable(false);
        grid.GetVertex(1, 1).SetIsWalkable(false);
        dStarLite.FindPath(v_start, v_goal);

        Assert.AreEqual(null, dStarLite.FindNext(v_start));
    }

    [Test]
    public void OptimalPath_OpenGrid_StraightDiagonalPath()
    {
        dStarLite.FindPath(v_start, v_goal);
        var path = GetPath(v_start, v_goal);
        var expected = new List<Vertex>
        {
            grid.GetVertex(0, 0),
            grid.GetVertex(1, 1),
            grid.GetVertex(2, 2),
            grid.GetVertex(3, 3),
            grid.GetVertex(4, 4),
        };

        Assert.AreEqual(expected.Count, path.Count);
        for (int i = 0; i < expected.Count; i++)
            Assert.AreEqual(expected[i], path[i]);
        Assert.AreEqual(4 * 14, v_start.gCost);
    }

    [Test]
    public void OptimalPath_VerticalWall_FindsPath()
    {
        var walls = new List<Vertex>
        {
            grid.GetVertex(2, 1),
            grid.GetVertex(2, 2),
            grid.GetVertex(2, 3),
            grid.GetVertex(2, 4)
        };
        foreach (var wall in walls)
            wall.SetIsWalkable(false);

        v_start = grid.GetVertex(0, 2);
        v_goal = grid.GetVertex(4, 2);
        dStarLite.FindPath(v_start, v_goal);
        var path = GetPath(v_start, v_goal);
        var expected = new List<Vertex>
        {
            grid.GetVertex(0, 2),
            grid.GetVertex(1, 1),
            grid.GetVertex(2, 0),
            grid.GetVertex(3, 1),
            grid.GetVertex(4, 2),
        };

        Assert.AreEqual(expected.Count, path.Count);
        for (int i = 0; i < expected.Count; i++)
            Assert.AreEqual(expected[i], path[i]);
        Assert.AreEqual(4 * 14, v_start.gCost);
    }

    [Test]
    public void OptimalPath_ComplexGrid_FindsPath()
    {
        var walls = new List<Vertex>
        {
            grid.GetVertex(1, 0),
            grid.GetVertex(1, 1),
            grid.GetVertex(1, 2),
            grid.GetVertex(1, 3),
            grid.GetVertex(3, 1),
            grid.GetVertex(3, 2),
            grid.GetVertex(3, 3),
            grid.GetVertex(3, 4),
        };
        foreach (var wall in walls)
            wall.SetIsWalkable(false);

        dStarLite.FindPath(v_start, v_goal);
        var path = GetPath(v_start, v_goal);
        var expected = new List<Vertex>
        {
            grid.GetVertex(0, 0),
            grid.GetVertex(0, 1),
            grid.GetVertex(0, 2),
            grid.GetVertex(0, 3),
            grid.GetVertex(1, 4),
            grid.GetVertex(2, 3),
            grid.GetVertex(2, 2),
            grid.GetVertex(2, 1),
            grid.GetVertex(3, 0),
            grid.GetVertex(4, 1),
            grid.GetVertex(4, 2),
            grid.GetVertex(4, 3),
            grid.GetVertex(4, 4)
        };

        Assert.AreEqual(expected.Count, path.Count);
        for (int i = 0; i < expected.Count; i++)
            Assert.AreEqual(expected[i], path[i]);
        Assert.AreEqual((8 * 10) + (4 * 14), v_start.gCost);
    }

    [Test]
    public void MovingAgent_UpdatesPath()
    {
        dStarLite.FindPath(v_start, v_goal);

        Vertex newStart = grid.GetVertex(1, 1);
        dStarLite.UpdateAgentPosition(newStart);

        var path = GetPath(newStart, v_goal);
        Assert.AreEqual(3 * 14, newStart.gCost);
    }

    [Test]
    public void DynamicObstacle_PathBlocked()
    {
        dStarLite.FindPath(v_start, v_goal);
        dStarLite.UpdateAgentPosition(grid.GetVertex(1, 1));
        grid.GetVertex(3, 4).SetIsWalkable(false);
        grid.GetVertex(4, 3).SetIsWalkable(false);
        grid.GetVertex(3, 3).SetIsWalkable(false);
        dStarLite.ChangeVertex(grid.GetVertex(3, 4));
        dStarLite.ChangeVertex(grid.GetVertex(4, 3));
        dStarLite.ChangeVertex(grid.GetVertex(3, 3));

        Assert.AreEqual(null, dStarLite.FindNext(grid.GetVertex(1, 1)));
    }

    [Test]
    public void DynamicObstacle_ReplansPath()
    {
        dStarLite.FindPath(v_start, v_goal);
        dStarLite.UpdateAgentPosition(grid.GetVertex(1, 1));
        grid.GetVertex(2, 2).SetIsWalkable(false);
        dStarLite.ChangeVertex(grid.GetVertex(2, 2));
        Vertex nextMove = dStarLite.FindNext(grid.GetVertex(1, 1));

        Assert.IsTrue(nextMove == grid.GetVertex(2, 1) || nextMove == grid.GetVertex(1, 2));
    }

    [Test]
    public void DynamicObstacle_PathUnchanged()
    {
        dStarLite.FindPath(v_start, v_goal);
        dStarLite.UpdateAgentPosition(grid.GetVertex(1, 1));
        grid.GetVertex(4, 0).SetIsWalkable(false);
        dStarLite.ChangeVertex(grid.GetVertex(4, 0));

        Assert.AreEqual(grid.GetVertex(2, 2), dStarLite.FindNext(grid.GetVertex(1, 1)));
    }

    [Test]
    public void DynamicObstacle_NoPathToPath()
    {
        grid.GetVertex(1, 0).SetIsWalkable(false);
        grid.GetVertex(0, 1).SetIsWalkable(false);
        grid.GetVertex(1, 1).SetIsWalkable(false);
        dStarLite.FindPath(v_start, v_goal);
        Assert.AreEqual(int.MaxValue, v_start.gCost);
        Assert.AreEqual(null, dStarLite.FindNext(v_start));
        
        grid.GetVertex(1, 0).SetIsWalkable(true);
        grid.GetVertex(0, 1).SetIsWalkable(true);
        grid.GetVertex(1, 1).SetIsWalkable(true);
        dStarLite.ChangeVertex(grid.GetVertex(1, 0));
        dStarLite.ChangeVertex(grid.GetVertex(0, 1));
        dStarLite.ChangeVertex(grid.GetVertex(1, 1));

        Assert.AreEqual(grid.GetVertex(1, 1), dStarLite.FindNext(v_start));
    }

    [Test]
    public void NoPossiblePath_ReturnsEmpty()
    {
        for (int x = 0; x < 5; x++)
            grid.GetVertex(x, 2).SetIsWalkable(false);

        dStarLite.FindPath(v_start, v_goal);
        Assert.AreEqual(null, dStarLite.FindNext(v_start));
    }
}