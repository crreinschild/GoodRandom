namespace GoodRandom.Test;

[TestClass]
public class NeededBytesTest : GoodRandomGenerator
{
    
    public NeededBytesTest() : base(null)
    {
    }

    [TestMethod]
    public void NeededBytesMinIsOne()
    {
        var random = new NeededBytesTest();
        Assert.AreEqual(1, random.NeededBytes(2));
    }
    
    [TestMethod]
    public void NeededBytesNegativeIsOne()
    {
        var random = new NeededBytesTest();
        Assert.AreEqual(1, random.NeededBytes(-100));
    }

    [TestMethod]
    public void NeededBytesZeroIsOne()
    {
        var random = new NeededBytesTest();
        Assert.AreEqual(1, random.NeededBytes(0));
    }
    
    [TestMethod]
    public void NeededBytesMaxIsFour()
    {
        var random = new NeededBytesTest();
        Assert.AreEqual(4, random.NeededBytes(int.MaxValue));
    }
    
    [TestMethod]
    public void NeededBytesNegativeMaxIsMeh()
    {
        var random = new NeededBytesTest();
        Assert.AreEqual(1, random.NeededBytes(int.MinValue));
    }
    
    [TestMethod]
    public void NeededBytesIsOneMoreHigherWhenHighestFairIsMoreThanHalfOfMax()
    {
        var random = new NeededBytesTest();
        Assert.AreEqual(1,random.NeededBytes(20));
        Assert.AreEqual(1,random.NeededBytes(120));
        Assert.AreEqual(2,random.NeededBytes(130));
        Assert.AreEqual(2,random.NeededBytes(255));
        Assert.AreEqual(2,random.NeededBytes(270));
        Assert.AreEqual(2,random.NeededBytes(ushort.MaxValue / 2 - 10));
        Assert.AreEqual(3,random.NeededBytes(ushort.MaxValue / 2 + 10));
        Assert.AreEqual(3,random.NeededBytes(ushort.MaxValue - 10));
        Assert.AreEqual(3,random.NeededBytes(ushort.MaxValue + 10));
        Assert.AreEqual(3,random.NeededBytes(ushort.MaxValue * byte.MaxValue / 2 - 10));
        Assert.AreEqual(3,random.NeededBytes(ushort.MaxValue * byte.MaxValue / 2 + 10));
        Assert.AreEqual(4,random.NeededBytes(ushort.MaxValue * byte.MaxValue - 10));
        Assert.AreEqual(4,random.NeededBytes(ushort.MaxValue * byte.MaxValue + 10));
    }
}