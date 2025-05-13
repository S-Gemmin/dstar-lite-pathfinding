using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid
{
    public event Action<Vertex> OnVertexChanged;
    public event Action OnReset;

    #region Constants
    private readonly (int dx, int dy)[] directions = new (int, int)[] {
        (-1, 1),
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (0, -1),
        (-1, -1),
        (-1, 0)
    };

    private const int MOVE_DIAGONAL_COST = 14;
    private const int MOVE_STRAIGHT_COST = 10;
    #endregion

    public int width { get; private set; }
    public int height { get; private set; }

    private Vertex[,] grid;

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new Vertex[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Vertex(x, y);
                grid[x, y].OnVertexChanged += Grid_OnVertexChanged;
            }
        }
    }

    public bool Inbounds(int x, int y)
        => 0 <= x && x < width && 0 <= y && y < height;

    public Vertex GetVertex(int x, int y)
    {
        if (!Inbounds(x, y))
            throw new IndexOutOfRangeException($"Grid position ({x},{y}) is out of bounds!");

        return grid[x, y];
    }

    public List<Vertex> GetNeighbors(Vertex vertex)
    {
        List<Vertex> neighbors = new List<Vertex>(8);

        foreach (var direction in directions)
        {
            int nx = vertex.x + direction.dx;
            int ny = vertex.y + direction.dy;

            if (!Inbounds(nx, ny))
                continue;

            Vertex neighbor = GetVertex(nx, ny);
            if (neighbor.isWalkable)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    public void Reset()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y].ResetCosts();

        OnReset?.Invoke();
    }

    public int h(Vertex v, Vertex u) // follows triangle inequality
    {
        int xDistance = Math.Abs(v.x - u.x);
        int yDistance = Math.Abs(v.y - u.y);
        int remaining = Math.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Math.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public int c(Vertex v, Vertex u)
    {
        int dx = Mathf.Abs(v.x - u.x);
        int dy = Mathf.Abs(v.y - u.y);

        if (dx > 1 || dy > 1 || (dx == 0 && dy == 0))
            throw new ArgumentException($"{v} and {u} are not neighbors!");

        return dx + dy == 1 ? MOVE_STRAIGHT_COST : MOVE_DIAGONAL_COST;
    }

    private void Grid_OnVertexChanged(Vertex vertex)
    {
        OnVertexChanged?.Invoke(vertex);
    }
}