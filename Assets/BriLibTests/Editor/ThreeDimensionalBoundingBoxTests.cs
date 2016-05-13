using NUnit.Framework;
using BriLib;

public class ThreeDimensionalBoundingBoxTests
{
    private ThreeDimensionalBoundingBox _box;

    [SetUp]
    public void Setup()
    {
        _box = new ThreeDimensionalBoundingBox(10, 10, 10, 10);
    }

    [Test]
    public void BoundsPointIntersection3D()
    {
        Assert.True(_box.Intersects(5, 5, 5));
        Assert.True(_box.Intersects(15, 15, 15));
    }

    [Test]
    public void BoundsPointNonIntersection3D()
    {
        Assert.False(_box.Intersects(-1, -1, -1));
        Assert.False(_box.Intersects(21, 21, 21));
    }

    [Test]
    public void BoundsBoundsEdgeIntersection3D()
    {
        var newBox = new ThreeDimensionalBoundingBox(15, 15, 15, 10);
        Assert.True(_box.Intersects(newBox));
    }

    [Test]
    public void BoundsBoundsInnerIntersection3D()
    {
        var newBox = new ThreeDimensionalBoundingBox(11, 11, 11, 2);
        Assert.True(_box.Intersects(newBox));
    }

    [Test]
    public void BoundsBoundsNonIntersections3D()
    {
        var newBox = new ThreeDimensionalBoundingBox(-5, -5, -5, 3);
        Assert.False(_box.Intersects(newBox));
    }
}
