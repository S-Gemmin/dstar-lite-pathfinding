using System;
using System.Collections.Generic;

public class MinHeapMap
{
    public struct Node
    {
        public Vertex vertex;
        public Key key;

        public Node(Vertex vertex, Key key)
        {
            this.vertex = vertex;
            this.key = key;
        }
    }

    private readonly List<Node> heap;
    private readonly Dictionary<Vertex, int> vertexToIndex;

    public MinHeapMap()
    {
        heap = new List<Node>();
        vertexToIndex = new Dictionary<Vertex, int>();
    }

    public MinHeapMap(IEnumerable<Node> nodes) : this()
    {
        foreach (Node node in nodes)
            Insert(node.vertex, node.key);
    }

    public int Count => heap.Count;
    public bool Empty => Count == 0;

    public Vertex Top()
    {
        if (Empty)
            throw new InvalidOperationException("Heap is empty!");
        return heap[0].vertex;
    }

    public Key TopKey()
    {
        if (Empty)
            throw new InvalidOperationException("Heap is empty!");
        return heap[0].key;
    }

    public bool Contains(Vertex vertex) => vertexToIndex.ContainsKey(vertex);

    public void Insert(Vertex vertex, Key key)
    {
        if (Contains(vertex))
            throw new InvalidOperationException($"Duplicate Vertex {vertex}!");

        Node node = new Node(vertex, key);
        heap.Add(node);
        vertexToIndex[vertex] = Count - 1;
        SiftUp(Count - 1);
    }

    public Vertex Pop()
    {
        if (Empty)
            throw new InvalidOperationException("Heap is empty!");
        Vertex top = Top();
        Remove(Top());
        return top; 
    }

    public void Remove(Vertex vertex)
    {
        if (!Contains(vertex))
            throw new InvalidOperationException($"Vertex {vertex} not found!");

        int index = vertexToIndex[vertex];
        Swap(index, Count - 1);
        heap.RemoveAt(Count - 1);
        vertexToIndex.Remove(vertex);

        if (index < Count)
        {
            SiftUp(index);
            SiftDown(index);
        }
    }

    public void Update(Vertex vertex, Key newKey)
    {
        if (!Contains(vertex))
            throw new InvalidOperationException($"Vertex {vertex} not found!");

        int index = vertexToIndex[vertex];
        Key oldKey = heap[index].key;
        heap[index] = new Node(vertex, newKey);

        SiftUp(index);
        SiftDown(index);
    }

    public void Reset()
    {
        heap.Clear();
        vertexToIndex.Clear();
    }

    private int Left(int i) => 2 * i + 1;
    private int Right(int i) => 2 * i + 2;
    private int Parent(int i) => (i - 1) / 2;

    private void SiftUp(int i)
    {
        while (i > 0)
        {
            int parent = Parent(i);
            if (heap[parent].key > heap[i].key)
            {
                Swap(i, parent);
                i = parent;
            }
            else
            {
                break;
            }
        }
    }

    private void SiftDown(int i)
    {
        while (true)
        {
            int smallest = i;
            int left = Left(i);
            int right = Right(i);

            if (left < Count && heap[left].key < heap[smallest].key)
                smallest = left;

            if (right < Count && heap[right].key < heap[smallest].key)
                smallest = right;

            if (smallest != i)
            {
                Swap(i, smallest);
                i = smallest;
            }
            else
            {
                break;
            }
        }
    }

    private void Swap(int i, int j)
    {
        (heap[i], heap[j]) = (heap[j], heap[i]);
        vertexToIndex[heap[i].vertex] = i;
        vertexToIndex[heap[j].vertex] = j;
    }
}
