using NUnit.Framework;
using System;

public class MinHeapMapTests
{
    private MinHeapMap heap;
    private Vertex v0_0, v0_1, v1_0, v1_1, v2_2, v3_3, v4_4, v5_5;

    [SetUp]
    public void SetUp()
    {
        heap = new MinHeapMap();

        v0_0 = new Vertex(0, 0);
        v0_1 = new Vertex(0, 1);
        v1_0 = new Vertex(1, 0);
        v1_1 = new Vertex(1, 1);
        v2_2 = new Vertex(2, 2);
        v3_3 = new Vertex(3, 3);
        v4_4 = new Vertex(4, 4);
        v5_5 = new Vertex(5, 5);
    }

    #region Constructor Tests
    [Test]
    public void Constructor_Empty_CreatesValidEmptyHeap()
    {
        Assert.AreEqual(0, heap.Count);
        Assert.IsTrue(heap.Empty);
        Assert.Throws<InvalidOperationException>(() => heap.Top());
    }

    [Test]
    public void Constructor_WithSingleNode_CreatesValidHeap()
    {
        var nodes = new[] { new MinHeapMap.Node(v1_1, new Key(1, 1)) };
        var singleHeap = new MinHeapMap(nodes);

        Assert.AreEqual(1, singleHeap.Count);
        Assert.AreEqual(v1_1, singleHeap.Top());
    }

    [Test]
    public void Constructor_WithMultipleNodes_CreatesValidHeap()
    {
        var nodes = new[]
        {
            new MinHeapMap.Node(v2_2, new Key(2, 2)),
            new MinHeapMap.Node(v1_1, new Key(1, 1)),
            new MinHeapMap.Node(v3_3, new Key(3, 3))
        };
        var multiHeap = new MinHeapMap(nodes);

        Assert.AreEqual(3, multiHeap.Count);
        Assert.AreEqual(v1_1, multiHeap.Top());
    }

    [Test]
    public void Constructor_WithDuplicateVertices_ThrowsException()
    {
        var nodes = new[]
        {
            new MinHeapMap.Node(v1_1, new Key(1, 1)),
            new MinHeapMap.Node(v1_1, new Key(2, 2))
        };

        Assert.Throws<InvalidOperationException>(() => new MinHeapMap(nodes));
    }
    #endregion

    #region Insert Tests
    [Test]
    public void Insert_IntoEmptyHeap_BecomesTop()
    {
        heap.Insert(v1_1, new Key(1, 1));
        Assert.AreEqual(v1_1, heap.Top());
        Assert.AreEqual(1, heap.Count);
    }

    [Test]
    public void Insert_MultipleElementsInOrder_MaintainsHeapProperty()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v3_3, new Key(3, 3));

        Assert.AreEqual(v1_1, heap.Top());
        Assert.AreEqual(3, heap.Count);
    }

    [Test]
    public void Insert_MultipleElementsReverseOrder_MaintainsHeapProperty()
    {
        heap.Insert(v3_3, new Key(3, 3));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v1_1, new Key(1, 1));

        Assert.AreEqual(v1_1, heap.Top());
        Assert.AreEqual(3, heap.Count);
    }

    [Test]
    public void Insert_MultipleElementsRandomOrder_MaintainsHeapProperty()
    {
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v4_4, new Key(4, 4));
        heap.Insert(v3_3, new Key(3, 3));
        heap.Insert(v0_0, new Key(0, 0));

        Assert.AreEqual(v0_0, heap.Top());
        Assert.AreEqual(5, heap.Count);
    }

    [Test]
    public void Insert_DuplicateVertex_ThrowsException()
    {
        heap.Insert(v1_1, new Key(1, 1));
        Assert.Throws<InvalidOperationException>(() => heap.Insert(v1_1, new Key(2, 2)));
    }

    [Test]
    public void Insert_WithSameKeyDifferentVertices_WorksCorrectly()
    {
        heap.Insert(v1_0, new Key(1, 1));
        heap.Insert(v0_1, new Key(1, 1));

        Assert.AreEqual(2, heap.Count);
        // Either could be on top since keys are equal
        Assert.IsTrue(heap.Top() == v1_0 || heap.Top() == v0_1);
    }
    #endregion

    #region Top/TopKey Tests
    [Test]
    public void Top_EmptyHeap_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => heap.Top());
    }

    [Test]
    public void Top_SingleElementHeap_ReturnsThatElement()
    {
        heap.Insert(v1_1, new Key(1, 1));
        Assert.AreEqual(v1_1, heap.Top());
    }

    [Test]
    public void Top_MultipleElements_ReturnsMinKeyElement()
    {
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v3_3, new Key(3, 3));

        Assert.AreEqual(v1_1, heap.Top());
        
    }

    [Test]
    public void TopKey_EmptyHeap_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => heap.TopKey());
    }

    [Test]
    public void TopKey_ReturnsCorrectKey()
    {
        var key = new Key(1, 1);
        heap.Insert(v1_1, key);
        Assert.AreEqual(key, heap.TopKey());
    }

    [Test]
    public void TopKey_AfterUpdate_ReturnsNewKey()
    {
        heap.Insert(v1_1, new Key(2, 2));
        heap.Update(v1_1, new Key(1, 1));
        Assert.AreEqual(new Key(1, 1), heap.TopKey());
    }
    #endregion

    #region Pop Tests
    [Test]
    public void Pop_EmptyHeap_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => heap.Pop());
    }

    [Test]
    public void Pop_RemovesAndReturnsTopElement()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v0_0, new Key(0, 0));

        Vertex topBefore = heap.Top();
        heap.Pop();

        Assert.AreEqual(v0_0, topBefore);
        Assert.AreEqual(v1_1, heap.Top());
        Assert.AreEqual(2, heap.Count);
        Assert.IsFalse(heap.Contains(v0_0));
    }

    [Test]
    public void Pop_MaintainsHeapProperty()
    {
        heap.Insert(v3_3, new Key(3, 3));
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v4_4, new Key(4, 4));
        heap.Insert(v0_0, new Key(0, 0));
        heap.Insert(v2_2, new Key(2, 2));

        Assert.AreEqual(v0_0, heap.Pop());
        Assert.AreEqual(v1_1, heap.Pop());
        Assert.AreEqual(v2_2, heap.Pop());
        Assert.AreEqual(v3_3, heap.Pop());
        Assert.AreEqual(v4_4, heap.Pop());
    }

    [Test]
    public void Pop_AfterReset_ThrowsException()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Reset();

        Assert.Throws<InvalidOperationException>(() => heap.Pop());
    }
    #endregion

    #region Remove Tests
    [Test]
    public void Remove_OnlyElement_HeapBecomesEmpty()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Remove(v1_1);

        Assert.AreEqual(0, heap.Count);
        Assert.IsTrue(heap.Empty);
    }

    [Test]
    public void Remove_RootElement_NewRootIsNextMin()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v3_3, new Key(3, 3));

        heap.Remove(v1_1);
        Assert.AreEqual(v2_2, heap.Top());
    }

    [Test]
    public void Remove_LeafElement_HeapMaintainsStructure()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v3_3, new Key(3, 3));

        heap.Remove(v3_3);
        Assert.AreEqual(2, heap.Count);
        Assert.AreEqual(v1_1, heap.Top());
    }

    [Test]
    public void Remove_MiddleElement_HeapMaintainsStructure()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v3_3, new Key(3, 3));
        heap.Insert(v4_4, new Key(4, 4));

        heap.Remove(v2_2);
        Assert.AreEqual(3, heap.Count);
        Assert.AreEqual(v1_1, heap.Top());
        Assert.IsFalse(heap.Contains(v2_2));
    }

    [Test]
    public void Remove_NonExistentElement_ThrowsException()
    {
        heap.Insert(v1_1, new Key(1, 1));
        Assert.Throws<InvalidOperationException>(() => heap.Remove(v2_2));
    }

    [Test]
    public void Remove_AllElements_HeapBecomesEmpty()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));

        heap.Remove(v1_1);
        heap.Remove(v2_2);

        Assert.AreEqual(0, heap.Count);
        Assert.IsTrue(heap.Empty);
    }
    #endregion

    #region Update Tests
    [Test]
    public void Update_OnlyElement_KeyChanges()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Update(v1_1, new Key(2, 2));

        Assert.AreEqual(new Key(2, 2), heap.TopKey());
    }

    [Test]
    public void Update_RootWithSmallerKey_RemainsRoot()
    {
        heap.Insert(v1_1, new Key(2, 2));
        heap.Insert(v2_2, new Key(3, 3));

        heap.Update(v1_1, new Key(1, 1));
        Assert.AreEqual(v1_1, heap.Top());
    }

    [Test]
    public void Update_RootWithLargerKey_NewRootSelected()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));

        heap.Update(v1_1, new Key(3, 3));
        Assert.AreEqual(v2_2, heap.Top());
    }

    [Test]
    public void Update_LeafWithSmallerKey_MovesUp()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v3_3, new Key(3, 3));

        heap.Update(v3_3, new Key(0, 0));
        Assert.AreEqual(v3_3, heap.Top());
        
    }

    [Test]
    public void Update_NonExistentVertex_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => heap.Update(v1_1, new Key(1, 1)));
    }

    [Test]
    public void Update_ToSameKey_NoChangeInHeap()
    {
        heap.Insert(v1_1, new Key(1, 1));
        var before = heap.TopKey();
        heap.Update(v1_1, new Key(1, 1));
        var after = heap.TopKey();

        Assert.AreEqual(before, after);
    }

    [Test]
    public void Update_MultipleTimes_HeapMaintainsStructure()
    {
        heap.Insert(v1_1, new Key(5, 5));
        heap.Insert(v2_2, new Key(6, 6));

        heap.Update(v1_1, new Key(4, 4));
        heap.Update(v2_2, new Key(3, 3));
        heap.Update(v1_1, new Key(2, 2));
        heap.Update(v2_2, new Key(1, 1));

        Assert.AreEqual(v2_2, heap.Top());
    }
    #endregion

    #region Contains/Count/Empty Tests
    [Test]
    public void Contains_EmptyHeap_ReturnsFalse()
    {
        Assert.IsFalse(heap.Contains(v1_1));
        
    }

    [Test]
    public void Contains_AfterInsert_ReturnsTrue()
    {
        heap.Insert(v1_1, new Key(1, 1));
        Assert.IsTrue(heap.Contains(v1_1));
    }

    [Test]
    public void Contains_AfterRemove_ReturnsFalse()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Remove(v1_1);
        Assert.IsFalse(heap.Contains(v1_1));
    }

    [Test]
    public void Count_ReflectsOperationsCorrectly()
    {
        Assert.AreEqual(0, heap.Count);

        heap.Insert(v1_1, new Key(1, 1));
        Assert.AreEqual(1, heap.Count);

        heap.Insert(v2_2, new Key(2, 2));
        Assert.AreEqual(2, heap.Count);

        heap.Remove(v1_1);
        Assert.AreEqual(1, heap.Count);

        heap.Remove(v2_2);
        Assert.AreEqual(0, heap.Count);
    }

    [Test]
    public void Empty_ReflectsOperationsCorrectly()
    {
        Assert.IsTrue(heap.Empty);

        heap.Insert(v1_1, new Key(1, 1));
        Assert.IsFalse(heap.Empty);

        heap.Remove(v1_1);
        Assert.IsTrue(heap.Empty);
    }
    #endregion

    #region Reset Tests
    [Test]
    public void Reset_EmptyHeap_DoesNothing()
    {
        heap.Reset();
        Assert.AreEqual(0, heap.Count);
        Assert.IsTrue(heap.Empty);
    }

    [Test]
    public void Reset_NonEmptyHeap_ClearsAllElements()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v2_2, new Key(2, 2));

        heap.Reset();

        Assert.AreEqual(0, heap.Count);
        Assert.IsTrue(heap.Empty);
        Assert.IsFalse(heap.Contains(v1_1));
        Assert.IsFalse(heap.Contains(v2_2));
    }

    [Test]
    public void Reset_AllowsReuseOfHeap()
    {
        heap.Insert(v1_1, new Key(1, 1));
        heap.Reset();

        heap.Insert(v0_0, new Key(0, 0));
        Assert.AreEqual(v0_0, heap.Top());
        Assert.AreEqual(1, heap.Count);
    }
    #endregion

    #region Complex Scenario Tests
    [Test]
    public void ComplexOperations_HeapMaintainsProperties()
    {
        heap.Insert(v3_3, new Key(3, 3));
        heap.Insert(v1_1, new Key(1, 1));
        heap.Insert(v4_4, new Key(4, 4));

        Assert.AreEqual(v1_1, heap.Top());
        Assert.AreEqual(3, heap.Count);

        heap.Update(v3_3, new Key(0, 2));
        Assert.AreEqual(v3_3, heap.Top());

        heap.Insert(v2_2, new Key(2, 2));
        heap.Insert(v5_5, new Key(5, 5));

        Assert.AreEqual(5, heap.Count);
        Assert.AreEqual(v3_3, heap.Top());

        heap.Remove(v1_1);
        Assert.AreEqual(4, heap.Count);
        Assert.IsFalse(heap.Contains(v1_1));

        heap.Update(v5_5, new Key(0, 1));
        Assert.AreEqual(v5_5, heap.Top());
    }
    #endregion
}