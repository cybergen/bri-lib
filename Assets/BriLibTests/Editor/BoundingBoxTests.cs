using NUnit.Framework;
using BriLib;

public class BoundingBoxTests
{
    private AxisAlignedBoundingBox _box;

    [SetUp]
    public void Setup()
    {
        _box = new AxisAlignedBoundingBox(10, 10, 10);
    }

    [Test]
    public void BoundsPointIntersection()
    {
        Assert.True(_box.Intersects(5, 5));
        Assert.True(_box.Intersects(15, 15));
    }

    [Test]
    public void BoundsPointNonIntersection()
    {
        Assert.False(_box.Intersects(-1, -1));
        Assert.False(_box.Intersects(21, 21));
    }

    [Test]
    public void BoundsBoundsEdgeIntersection()
    {
        var newBox = new AxisAlignedBoundingBox(15, 15, 10);
        Assert.True(_box.Intersects(newBox));
    }

    [Test]
    public void BoundsBoundsInnerIntersection()
    {
        var newBox = new AxisAlignedBoundingBox(11, 11, 2);
        Assert.True(_box.Intersects(newBox));
    }

    [Test]
    public void BoundsBoundsNonIntersections()
    {
        var newBox = new AxisAlignedBoundingBox(-5, -5, 3);
        Assert.False(_box.Intersects(newBox));
    }
}
