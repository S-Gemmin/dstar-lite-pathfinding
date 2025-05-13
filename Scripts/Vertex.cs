using System;

public class Vertex // hCost & k1 used for ui. Can use dictionary to store g/rhsCosts.
{
    public event Action<Vertex> OnVertexChanged;
    public int x { get; private set; }
    public int y { get; private set; }
    public bool isWalkable { get; private set; }
    public int gCost { get; private set; }
    public int rhsCost { get; private set; }
    public int hCost { get; private set; } // -1 == n/a
    public int k1Cost { get; private set; } // -1 == n/a

    public Vertex(int x, int y, bool isWalkable = true, int gCost = int.MaxValue, int rhsCost = int.MaxValue, int hCost = -1, int k1Cost = -1)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
        this.gCost = gCost;
        this.rhsCost = rhsCost;
        this.hCost = hCost;
        this.k1Cost = k1Cost;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        if (this.isWalkable == isWalkable) return;
        this.isWalkable = isWalkable;
        OnVertexChanged?.Invoke(this);
    }

    public void SetGCost(int gCost)
    {
        if (gCost < 0)
            throw new ArgumentOutOfRangeException(nameof(gCost));

        this.gCost = gCost;
        OnVertexChanged?.Invoke(this);
    }

    public void SetRhsCost(int rhsCost)
    {
        if (rhsCost < 0)
            throw new ArgumentOutOfRangeException(nameof(rhsCost));

        this.rhsCost = rhsCost;
        OnVertexChanged?.Invoke(this);
    }

    public void Update(int hCost, int k1Cost)
    {
        this.hCost = hCost;
        this.k1Cost = k1Cost;
        OnVertexChanged?.Invoke(this);
    }

    public void ResetCosts()
    {
        gCost = int.MaxValue;
        rhsCost = int.MaxValue;
        hCost = -1;
        k1Cost = -1;  
    }

    public bool Equals(Vertex other) =>
        other != null ? x == other.x && y == other.y : false;
    
    public override bool Equals(object obj) => Equals(obj as Vertex);
    public override int GetHashCode() => (x, y).GetHashCode();
    public static bool operator ==(Vertex left, Vertex right) => Equals(left, right);
    public static bool operator !=(Vertex left, Vertex right) => !Equals(left, right);
    public override string ToString() 
        => $"(Vertex (X: {x}, Y: {y}) | isWalkable: {isWalkable} | g: {gCost} | rhs: {rhsCost} | h: {hCost} | k1: {k1Cost})";
}
