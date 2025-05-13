using NUnit.Framework;
using System;

public class VertexTests
{
    private Vertex vertex;
    private Vertex eventVertex;

    [SetUp]
    public void SetUp()
    {
        vertex = new Vertex(1, 2);

        vertex.OnVertexChanged += (changedVertex) =>
        {
            eventVertex = changedVertex;
        };
    }

    [Test]
    public void Initialize_SetsPropertiesCorrectly()
    {
        Assert.AreEqual(1, vertex.x);
        Assert.AreEqual(2, vertex.y);
        Assert.IsTrue(vertex.isWalkable);
        Assert.AreEqual(int.MaxValue, vertex.gCost);
        Assert.AreEqual(int.MaxValue, vertex.rhsCost);
    }

    [Test]
    public void Initialize_WithWalkableParameter_SetsCorrectly()
    {
        vertex = new Vertex(1, 2, false);
        Assert.IsFalse(vertex.isWalkable);
    }

    [Test]
    public void SetGCost_ChangesValueAndUpdatesText()
    {
        vertex.SetGCost(10);
        Assert.AreEqual(10, vertex.gCost);
    }

    [Test]
    public void SetGCost_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => vertex.SetGCost(-1));
    }

    [Test]
    public void SetRhsCost_ChangesValueAndUpdatesText()
    {
        vertex.SetRhsCost(5);
        Assert.AreEqual(5, vertex.rhsCost);
    }

    [Test]
    public void SetRhsCost_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => vertex.SetRhsCost(-1));
    }

    [Test]
    public void ResetCosts_SetsCostsToMaxValue()
    {
        vertex.SetGCost(10);
        vertex.SetRhsCost(5);
        vertex.ResetCosts();
        Assert.AreEqual(int.MaxValue, vertex.gCost);
        Assert.AreEqual(int.MaxValue, vertex.rhsCost);
    }

    [Test]
    public void Equals_WithSameCoordinates_ReturnsTrue()
    {
        Vertex otherVertex = new Vertex(1, 2);
        Assert.IsTrue(vertex.Equals(otherVertex));
        Assert.IsTrue(vertex == otherVertex);
        Assert.IsFalse(vertex != otherVertex);
    }

    [Test]
    public void Equals_WithDifferentCoordinates_ReturnsFalse()
    {
        Vertex otherVertex = new Vertex(3, 4);
        Assert.IsFalse(vertex.Equals(otherVertex));
        Assert.IsFalse(vertex == otherVertex);
        Assert.IsTrue(vertex != otherVertex);
    }

    [Test]
    public void Equals_WithNull_ReturnsFalse()
    {
        Assert.IsFalse(vertex.Equals(null));
        Assert.IsFalse(vertex == null);
        Assert.IsFalse(null == vertex);
        Assert.IsTrue(vertex != null);
        Assert.IsTrue(null != vertex);
    }

    [Test]
    public void GetHashCode_ReturnsConsistentValue()
    {
        var expectedHash = (1, 2).GetHashCode();
        Assert.AreEqual(expectedHash, vertex.GetHashCode());
    }

    [Test]
    public void ToString_ReturnsCorrectFormat()
    {
        vertex.SetGCost(10);
        vertex.SetRhsCost(5);
        var result = vertex.ToString();
        StringAssert.Contains("Vertex (X: 1, Y: 2)", result);
        StringAssert.Contains("isWalkable: True", result);
        StringAssert.Contains("g: 10", result);
        StringAssert.Contains("rhs: 5", result);
    }

    [Test]
    public void PropertyChanges_TriggerEvent_WithUpdatedVertex()
    {
        vertex.SetIsWalkable(false);
        Assert.AreEqual(vertex, eventVertex);
        Assert.IsFalse(eventVertex.isWalkable);

        vertex.SetGCost(10);
        Assert.AreEqual(vertex, eventVertex);
        Assert.AreEqual(10, eventVertex.gCost);
    }
}