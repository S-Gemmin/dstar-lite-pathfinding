using System.Collections.Generic;
using UnityEngine;

public class GridVisual : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] private VertexVisual pfVertexVisual;
    [SerializeField] private int vertexSize = 10;
    [Header("Debug")]
    [SerializeField] private bool showGridGizmos = true;
    [SerializeField] private Color gizmoColor = Color.white;

    private static readonly Color black = new Color(0, 0, 0); // unwalkable
    private static readonly Color grey = new Color(0.39f, 0.39f, 0.39f); // walkable (not yet visited)
    private static readonly Color red = new Color(1, 0, 0); // inconsistent (in heap)
    private static readonly Color cyan = new Color(0, 0.6f, 1f); // consistent
    private static readonly Color green = new Color(0, 1, 0); // path 
    private static readonly Color yellow = new Color(1.0f, 0.874f, 0.274f); // v_goal

    private Grid grid;
    private VertexVisual[,] vertexVisuals;
    private Queue<Vertex> snapshots;

    public void Setup(Grid grid)
    {
        snapshots = new Queue<Vertex>();
        this.grid = grid;

        vertexVisuals = new VertexVisual[grid.width, grid.height];

        for (int x = 0; x < grid.width; x++)
            for (int y = 0; y < grid.height; y++)
                vertexVisuals[x, y] = Instantiate(pfVertexVisual, GetWorldPosition(x, y), Quaternion.identity);

        DStarLite.i.OnUpdateVertex += AddSnapshot;
        DStarLite.i.OnGCostChanged += AddSnapshot;
        grid.OnReset += Reset;
    }

    public void HighlightTargetVertex(Vertex vertex)
    {
        vertexVisuals[vertex.x, vertex.y].SetBackgroundColor(yellow);
    }

    public void ShowNextSnapshot()
    {
        if (snapshots.Count > 0)
            UpdateVertexVisual(snapshots.Dequeue(), true);
    }

    public void UpdateVertexVisual(Vertex vertex, bool flashWhite = false)
    {
        VertexVisual vertexVisual = vertexVisuals[vertex.x, vertex.y];

        if (!vertex.isWalkable)
            vertexVisual.SetBackgroundColor(black);
        else if (vertex.gCost == int.MaxValue && vertex.rhsCost == int.MaxValue) // unvisited
            vertexVisual.SetBackgroundColor(grey);
        else if (vertex.gCost == vertex.rhsCost) // locally consistent
            vertexVisual.SetBackgroundColor(cyan);
        else // locally inconsistent
            vertexVisual.SetBackgroundColor(red);

        vertexVisual.SetGCost(vertex.gCost);
        vertexVisual.SetRhsCost(vertex.rhsCost);
        vertexVisual.UpdateVertex(vertex.hCost, vertex.k1Cost);

        if (flashWhite)
            StartCoroutine(vertexVisual.FlashWhite());
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        float gridWidth = grid.width * vertexSize;
        float gridHeight = grid.height * vertexSize;

        return transform.position
            - new Vector3(gridWidth * 0.5f, gridHeight * 0.5f)
            + new Vector3((x + 0.5f) * vertexSize, (y + 0.5f) * vertexSize);
    }

    public Vector3 GetWorldPosition(Vertex vertex)
        => GetWorldPosition(vertex.x, vertex.y);

    public Vertex GetVertex(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return grid.GetVertex(x, y);
    }     

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 localPosition = worldPosition - transform.position;
        float gridWidth = grid.width * vertexSize;
        float gridHeight = grid.height * vertexSize;

        x = Mathf.Clamp(Mathf.FloorToInt((localPosition.x + gridWidth * 0.5f) / vertexSize), 0, grid.width - 1);
        y = Mathf.Clamp(Mathf.FloorToInt((localPosition.y + gridHeight * 0.5f) / vertexSize), 0, grid.height - 1);
    }

    private void AddSnapshot(Vertex vertex)
    {
        snapshots.Enqueue(vertex);
    }

    private void Reset() // need to reset hCost and k1Cost
    {
        for (int x = 0; x < grid.width; x++)
            for (int y = 0; y < grid.height; y++)
                UpdateVertexVisual(grid.GetVertex(x, y));

        snapshots.Clear();
    }

    private void OnDrawGizmos()
    {
        if (!showGridGizmos || grid == null) return;

        Gizmos.color = gizmoColor;
        Vector3 origin = transform.position - new Vector3(
            grid.width * vertexSize * 0.5f,
            grid.height * vertexSize * 0.5f
        );

        // Draw grid lines
        for (int x = 0; x <= grid.width; x++)
        {
            Vector3 start = origin + new Vector3(x * vertexSize, 0);
            Vector3 end = start + new Vector3(0, grid.height * vertexSize);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= grid.height; y++)
        {
            Vector3 start = origin + new Vector3(0, y * vertexSize);
            Vector3 end = start + new Vector3(grid.width * vertexSize, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}
