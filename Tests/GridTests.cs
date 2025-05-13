using NUnit.Framework;
using System;

public class GridTests
{
    private Grid grid;
    private Vertex v0_0, v1_0, v0_1, v1_1, v2_2;

    [SetUp]
    public void SetUp()
    {
        grid = new Grid(3, 3);
        v0_0 = grid.GetVertex(0, 0);
        v1_0 = grid.GetVertex(1, 0);
        v0_1 = grid.GetVertex(0, 1);
        v1_1 = grid.GetVertex(1, 1);
        v2_2 = grid.GetVertex(2, 2);
    }

    [Test]
    public void Inbounds_ValidCoordinates_ReturnsTrue()
    {
        Assert.IsTrue(grid.Inbounds(0, 0));
        Assert.IsTrue(grid.Inbounds(2, 2));
    }

    [Test]
    public void Inbounds_OutOfBounds_ReturnsFalse()
    {
        Assert.IsFalse(grid.Inbounds(-1, 0));
        Assert.IsFalse(grid.Inbounds(0, -1));
        Assert.IsFalse(grid.Inbounds(3, 0));
        Assert.IsFalse(grid.Inbounds(0, 3));
    }

    [Test]
    public void GetVertex_ValidCoordinates_ReturnsCorrectVertex()
    {
        Vertex vertex = grid.GetVertex(1, 1);
        Assert.AreEqual(1, vertex.x);
        Assert.AreEqual(1, vertex.y);
    }

    [Test]
    public void GetVertex_OutOfBounds_ThrowsException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => grid.GetVertex(-1, 0));
        Assert.Throws<IndexOutOfRangeException>(() => grid.GetVertex(3, 3));
    }

    [Test]
    public void GetNeighbors_CenterVertex_ReturnsAllWalkableNeighbors()
    {
        var neighbors = grid.GetNeighbors(v1_1);
        Assert.AreEqual(8, neighbors.Count);
    }

    [Test]
    public void GetNeighbors_CornerVertex_ReturnsValidNeighbors()
    {
        var neighbors = grid.GetNeighbors(v0_0);
        Assert.AreEqual(3, neighbors.Count);
        Assert.Contains(v1_0, neighbors);
        Assert.Contains(v0_1, neighbors);
        Assert.Contains(v1_1, neighbors);
    }

    [Test]
    public void GetNeighbors_UnwalkableVertex_ExcludesUnwalkable()
    {
        v1_0.SetIsWalkable(false);
        var neighbors = grid.GetNeighbors(v0_0);
        Assert.AreEqual(2, neighbors.Count);
        Assert.False(neighbors.Contains(v1_0));
    }

    [Test]
    public void Reset_ResetsAllVerticesToDefaultValues()
    {
        v0_0.SetGCost(10);
        v0_0.SetRhsCost(5);
        v1_1.SetGCost(20);
        v1_1.SetRhsCost(15);
        v2_2.SetGCost(30);
        v2_2.SetRhsCost(25);
        grid.Reset();

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                Vertex vertex = grid.GetVertex(x, y);
                Assert.AreEqual(int.MaxValue, vertex.gCost);
                Assert.AreEqual(int.MaxValue, vertex.rhsCost);
            }
        }
    }

    [Test]
    public void Reset_PreservesWalkableStatus()
    {
        v0_0.SetIsWalkable(false);
        v1_1.SetIsWalkable(false);
        grid.Reset();

        Assert.IsFalse(v0_0.isWalkable);
        Assert.IsFalse(v1_1.isWalkable);
        Assert.IsTrue(v2_2.isWalkable);
    }

    [Test]
    public void Reset_EmptyGrid_DoesNotThrow()
    {
        var emptyGrid = new Grid(0, 0);
        Assert.DoesNotThrow(() => emptyGrid.Reset());
    }

    [Test]
    public void Reset_AfterMultipleModifications()
    {
        v0_0.SetGCost(10);
        v0_0.SetRhsCost(5);
        grid.Reset();
        v1_1.SetGCost(8);
        v1_1.SetRhsCost(3);
        grid.Reset();

        Assert.AreEqual(int.MaxValue, v0_0.gCost);
        Assert.AreEqual(int.MaxValue, v0_0.rhsCost);
        Assert.AreEqual(int.MaxValue, v1_1.gCost);
        Assert.AreEqual(int.MaxValue, v1_1.rhsCost);
    }

    [Test]
    public void h_DiagonalDistance_CalculatesCorrectCost()
    {
        Assert.AreEqual(14, grid.h(v0_0, v1_1));
        Assert.AreEqual(28, grid.h(v0_0, v2_2));
    }

    [Test]
    public void h_StraightDistance_CalculatesCorrectCost()
    {
        Assert.AreEqual(10, grid.h(v0_0, v1_0));
        Assert.AreEqual(10, grid.h(v0_0, v0_1));
    }

    [Test]
    public void c_AdjacentVertices_ReturnsCorrectCost()
    {
        Assert.AreEqual(10, grid.c(v0_0, v1_0));
        Assert.AreEqual(10, grid.c(v0_0, v0_1));
        Assert.AreEqual(14, grid.c(v0_0, v1_1));
    }

    [Test]
    public void h_IsSymmetric()
    {
        Assert.AreEqual(grid.h(v0_0, v1_1), grid.h(v1_1, v0_0));
        Assert.AreEqual(grid.h(v1_0, v2_2), grid.h(v2_2, v1_0));
        Assert.AreEqual(grid.h(v0_1, v1_1), grid.h(v1_1, v0_1));
    }

    [Test]
    public void h_SelfDistance_IsZero()
    {
        Assert.AreEqual(0, grid.h(v0_0, v0_0));
        Assert.AreEqual(0, grid.h(v1_1, v1_1));
        Assert.AreEqual(0, grid.h(v2_2, v2_2));
    }

    [Test]
    public void c_ReturnsCorrectCostValues()
    {
        Assert.AreEqual(10, grid.c(v0_0, v1_0));
        Assert.AreEqual(10, grid.c(v0_0, v0_1));
        Assert.AreEqual(14, grid.c(v0_0, v1_1));
    }

    [Test]
    public void c_IsSymmetric_ForStraightNeighbors()
    {
        Assert.AreEqual(grid.c(v0_0, v1_0), grid.c(v1_0, v0_0));
        Assert.AreEqual(grid.c(v0_0, v0_1), grid.c(v0_1, v0_0));
    }

    [Test]
    public void c_IsSymmetric_ForDiagonalNeighbors()
    {
        Assert.AreEqual(grid.c(v0_0, v1_1), grid.c(v1_1, v0_0));
        Assert.AreEqual(grid.c(v1_0, v0_1), grid.c(v0_1, v1_0));
    }

    [Test]
    public void c_NonAdjacentVertices_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => grid.c(v0_0, v2_2));
        Assert.Throws<ArgumentException>(() => grid.c(v0_0, v0_0));
    }

    [Test]
    public void OnVertexChanged_PropagatesFromVertexToGrid()
    {
        bool eventFired = false;
        grid.OnVertexChanged += (v) => eventFired = true;

        v0_0.SetIsWalkable(false);
        Assert.IsTrue(eventFired);
    }
}