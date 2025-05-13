using NUnit.Framework;

[TestFixture]
public class KeyTests
{
    [Test]
    public void LessThanOperator_WhenK1IsSmaller_ReturnsTrue()
    {
        var smaller = new Key(1, 100);
        var larger = new Key(2, 0);

        Assert.IsTrue(smaller < larger);
    }

    [Test]
    public void LessThanOperator_WhenK1IsLarger_ReturnsFalse()
    {
        var larger = new Key(5, 0);
        var smaller = new Key(3, 100);

        Assert.IsFalse(larger < smaller);
    }

    [Test]
    public void LessThanOperator_WhenK1EqualAndK2IsSmaller_ReturnsTrue()
    {
        var smaller = new Key(10, 5);
        var larger = new Key(10, 10);

        Assert.IsTrue(smaller < larger);
    }

    [Test]
    public void LessThanOperator_WhenK1EqualAndK2IsLarger_ReturnsFalse()
    {
        var larger = new Key(10, 15);
        var smaller = new Key(10, 10);

        Assert.IsFalse(larger < smaller);
    }

    [Test]
    public void LessThanOperator_WhenKeysAreEqual_ReturnsFalse()
    {
        var key1 = new Key(7, 7);
        var key2 = new Key(7, 7);

        Assert.IsFalse(key1 < key2);
    }

    [Test]
    public void GreaterThanOperator_WhenK1IsLarger_ReturnsTrue()
    {
        var larger = new Key(2, 0);
        var smaller = new Key(1, 100);

        Assert.IsTrue(larger > smaller);
    }

    [Test]
    public void GreaterThanOperator_WhenK1IsSmaller_ReturnsFalse()
    {
        var smaller = new Key(3, 100);
        var larger = new Key(5, 0);

        Assert.IsFalse(smaller > larger);
    }

    [Test]
    public void GreaterThanOperator_WhenK1EqualAndK2IsLarger_ReturnsTrue()
    {
        var larger = new Key(10, 10);
        var smaller = new Key(10, 5);

        Assert.IsTrue(larger > smaller);
    }

    [Test]
    public void GreaterThanOperator_WhenK1EqualAndK2IsSmaller_ReturnsFalse()
    {
        var smaller = new Key(10, 10);
        var larger = new Key(10, 15);

        Assert.IsFalse(smaller > larger);
    }

    [Test]
    public void GreaterThanOperator_WhenKeysAreEqual_ReturnsFalse()
    {
        var key1 = new Key(7, 7);
        var key2 = new Key(7, 7);

        Assert.IsFalse(key1 > key2);
    }
}