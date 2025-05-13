using System.Collections.Generic;
using UnityEngine;
using System;

public class DStarLite
{
    // events are for ui. params should be all copies!
    public event Action<Vertex> OnUpdateVertex; 
    public event Action<Vertex> OnGCostChanged;
    public event Action<int> Onk_mChanged;

    public static DStarLite i { get; private set;  }
    public int k_m { get; private set;  }


    private Grid grid;
    private MinHeapMap minHeapMap;
    private Vertex v_goal; // target vertex
    private Vertex v_start; // agent's position
    private Vertex v_last; // last vertex since grid change

    public DStarLite(Grid grid)
    {
        this.grid = grid;
        minHeapMap = new MinHeapMap();
        i = this;
    }

    public void UpdateAgentPosition(Vertex v_start) => this.v_start = v_start;

    public void FindPath(Vertex v_start, Vertex v_goal) // reset everything since g != g* anymore
    {
        minHeapMap.Reset();

        this.v_start = v_start;
        this.v_goal = v_goal;
        
        v_last = v_start;
        k_m = 0;

        grid.Reset();
        v_goal.SetRhsCost(0);

        minHeapMap.Insert(v_goal, CalculateKey(v_goal));

        if (v_start.isWalkable && v_goal.isWalkable && v_start != v_goal)
            ComputeShortestPath();
    }

    public Vertex FindNext(Vertex v) // will return null if no path
    {
        if (v.gCost == int.MaxValue) return null;

        List<Vertex> neighbors = grid.GetNeighbors(v);
        int minG = int.MaxValue;
        Vertex next = null;

        foreach (Vertex neighbor in neighbors)
        {
            if (neighbor.gCost < minG)
            {
                minG = neighbor.gCost;
                next = neighbor;
            }
        }

        return next;
    }

    public void ChangeVertex(Vertex v)
    {
        if (v_last == null || v_start == null) return;
        k_m += grid.h(v_last, v_start);
        Onk_mChanged?.Invoke(k_m);
        v_last = v_start;
        v.SetRhsCost(v.isWalkable ? ComputeRhs(v) : int.MaxValue);
        UpdateVertex(v);
        ComputeShortestPath();
    }

    private Key CalculateKey(Vertex v)
    {
        int goalDistance = Mathf.Min(v.gCost, v.rhsCost);
        return goalDistance < int.MaxValue ? new Key(goalDistance + grid.h(v_start, v) + k_m, goalDistance) : new Key(int.MaxValue, int.MaxValue);
    }

    private void UpdateVertex(Vertex v)
    {
        if (minHeapMap.Contains(v) && v.gCost != v.rhsCost)
            minHeapMap.Update(v, CalculateKey(v));
        else if (!minHeapMap.Contains(v) && v.gCost != v.rhsCost)
            minHeapMap.Insert(v, CalculateKey(v));
        else if (minHeapMap.Contains(v) && v.gCost == v.rhsCost)
            minHeapMap.Remove(v);

        OnUpdateVertex?.Invoke(new Vertex(v.x, v.y, v.isWalkable, v.gCost, v.rhsCost, grid.h(v_start, v), CalculateKey(v).k1)); // for ui
    }

    private void ComputeShortestPath()
    {
        while (!minHeapMap.Empty && (minHeapMap.TopKey() < CalculateKey(v_start) || v_start.gCost != v_start.rhsCost))
        {
            Vertex v = minHeapMap.Top();
            Key oldKey = minHeapMap.TopKey();
            Key newKey = CalculateKey(v);

            if (oldKey < newKey)
                minHeapMap.Update(v, newKey);
            else if (v.gCost > v.rhsCost) // locally overconsistent
            {
                v.SetGCost(v.rhsCost);
                OnGCostChanged?.Invoke(new Vertex(v.x, v.y, v.isWalkable, v.gCost, v.rhsCost, grid.h(v_start, v), CalculateKey(v).k1));
                minHeapMap.Pop();
                List<Vertex> neighbors = grid.GetNeighbors(v);
                foreach (Vertex neighbor in neighbors)
                {
                    neighbor.SetRhsCost(Mathf.Min(neighbor.rhsCost, v.gCost + grid.c(v, neighbor)));
                    UpdateVertex(neighbor);
                }
            }
            else // locally underconsistent
            {
                int g_old = v.gCost;
                v.SetGCost(int.MaxValue);
                OnGCostChanged?.Invoke(new Vertex(v.x, v.y, v.isWalkable, v.gCost, v.rhsCost, grid.h(v_start, v), CalculateKey(v).k1));
                List<Vertex> neighbors = grid.GetNeighbors(v);
                foreach (Vertex neighbor in neighbors)
                {
                    if (neighbor.rhsCost == g_old + grid.c(v, neighbor))
                        neighbor.SetRhsCost(ComputeRhs(neighbor));
                    
                    UpdateVertex(neighbor);
                }

                UpdateVertex(v);
            }
        }
    }

    private int ComputeRhs(Vertex v)
    {
        if (v == v_goal) return 0;

        List<Vertex> neighbors = grid.GetNeighbors(v);
        int minRhs = int.MaxValue;
        
        foreach (Vertex neighbor in neighbors)
            if (neighbor.gCost < int.MaxValue)
                minRhs = Mathf.Min(minRhs, neighbor.gCost + grid.c(v, neighbor));

        return minRhs;
    }
}
