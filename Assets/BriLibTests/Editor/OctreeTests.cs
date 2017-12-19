using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using BriLib;

public class OctreeTests
{
    private Octree<TestObject> _tree;
    private ThreeDimensionalBoundingBox _box;
    private TestObject _fiveFiveObject = new TestObject();
    private TestObject _twoTwoObject = new TestObject();
    private TestObject _threeThreeObject = new TestObject();
    private TestObject _fourFourObject = new TestObject();
    private TestObject _fifteenFifteenObject = new TestObject();
    private TestObject _twentyFiveObject = new TestObject();
    private TestObject _negativeOneObject = new TestObject();

    [SetUp]
    public void Setup()
    {
        _box = new ThreeDimensionalBoundingBox(10, 10, 10, 20);
        _tree = new Octree<TestObject>(_box, 2);
    }

    [Test]
    public void GetOneObjectInRange3D()
    {
        _tree.Insert(5, 5, 5, _fiveFiveObject);
        var objects = _tree.GetRange(_box).ToList();
        Assert.AreEqual(1, objects.Count());
        Assert.AreEqual(_fiveFiveObject, objects[0]);
    }

    [Test]
    public void GetOneObjectInSubRange3D()
    {
        _tree.Insert(5, 5, 5, _fiveFiveObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(3, 3, 3, 5)).ToList();
        Assert.AreEqual(1, objects.Count());
        Assert.AreEqual(_fiveFiveObject, objects[0]);
    }

    [Test]
    public void GetNoObjectsInRange3D()
    {
        _tree.Insert(-1, -1, -1, _negativeOneObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(3, 3, 3, 2)).ToList();
        Assert.AreEqual(0, objects.Count());
    }

    [Test]
    public void GetNoObjectsInSubRange3D()
    {
        _tree.Insert(5, 5, 5, _fiveFiveObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(15, 15, 15, 5)).ToList();
        Assert.AreEqual(0, objects.Count());
    }

    [Test]
    public void GetObjectsAfterSubDivide3D()
    {
        _tree.Insert(4, 4, 4, _fourFourObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(3, 3, 3, _threeThreeObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(3, objects.Count);
        Assert.True(objects.Contains(_fourFourObject));
        Assert.True(objects.Contains(_twoTwoObject));
        Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void GetObjectsAcrossMultipleZonesAfterSubdivide3D()
    {
        _tree.Insert(15, 15, 15, _fifteenFifteenObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(3, 3, 3, _threeThreeObject);
        var objects = _tree.GetRange(_box).ToList();
        Assert.AreEqual(3, objects.Count);
        Assert.True(objects.Contains(_fifteenFifteenObject));
        Assert.True(objects.Contains(_twoTwoObject));
        Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void GetOneInRangeTreeWithOneItemPerCell()
    {
        _tree = new Octree<TestObject>(0, 0, 0, 10, 1);
        var inRangeObject = new TestObject();
        var outOfRangeObject = new TestObject();
        _tree.Insert(0, 0, 0, inRangeObject);
        _tree.Insert(1, 1, 1, outOfRangeObject);
        var objects = _tree.GetRange(0, 0, 0, 0.01f);
        Assert.AreEqual(1, objects.Count());
        Assert.True(objects.Contains(inRangeObject));
        Assert.False(objects.Contains(outOfRangeObject));
    }

    [Test]
    public void GetFourInRangeTreeWithOneItemPerCell()
    {
        _tree = new Octree<TestObject>(0, 0, 0, 10, 1);
        var inRangeObject = new TestObject();
        var inRangeObjectTwo = new TestObject();
        var inRangeObjectThree = new TestObject();
        var inRangeObjectFour = new TestObject();
         var outOfRangeObject = new TestObject();
        _tree.Insert(0, 0, 0, inRangeObject);
        _tree.Insert(0.2f, 0, 0, inRangeObjectTwo);
        _tree.Insert(0, 0.25f, 0, inRangeObjectThree);
        _tree.Insert(0, 0.2f, 0.25f, inRangeObjectFour);
        _tree.Insert(1, 1, 1, outOfRangeObject);
        var objects = _tree.GetRange(0, 0, 0, 0.6f);
        Assert.AreEqual(4, objects.Count());
        Assert.True(objects.Contains(inRangeObject));
        Assert.True(objects.Contains(inRangeObjectTwo));
        Assert.True(objects.Contains(inRangeObjectThree));
        Assert.True(objects.Contains(inRangeObjectFour));
        Assert.False(objects.Contains(outOfRangeObject));
    }

    [Test]
    public void GetTwentyInRangeTreeWithOneItemPerCell()
    {
        _tree = new Octree<TestObject>(0, 0, 0, 10, 1);
        var targetRadius = 1f;
        var rand = new System.Random();
        var inRangeList = new List<TestObject>();
        for (int i = 0; i < 20; i++)
        {
            var newObject = new TestObject();
            inRangeList.Add(newObject);
            var x = MathHelpers.GetRandom(targetRadius / 2f, rand);
            var y = MathHelpers.GetRandom(targetRadius / 2f, rand);
            var z = MathHelpers.GetRandom(targetRadius / 2f, rand);
            _tree.Insert(x, y, z, newObject);
        }
        var outOfRangeList = new List<TestObject>();
        var additionalRange = 6f;
        for (int i = 0; i < 20; i++)
        {
            var newObject = new TestObject();
            outOfRangeList.Add(newObject);
            var x = MathHelpers.GetRandom(additionalRange, rand) + targetRadius;
            var y = MathHelpers.GetRandom(additionalRange, rand) + targetRadius;
            var z = MathHelpers.GetRandom(additionalRange, rand) + targetRadius;
            _tree.Insert(x, y, z, newObject);
        }

        var objects = _tree.GetRange(0, 0, 0, targetRadius);
        Assert.AreEqual(20, objects.Count());

        for (int i = 0; i < inRangeList.Count; i++)
        {
            Assert.True(objects.Contains(inRangeList[i]));
        }

        for (int i = 0; i < outOfRangeList.Count; i++)
        {
           Assert.False(objects.Contains(outOfRangeList[i]));
        }
    }

    [Test]
    public void AddEntriesOutOfRange()
    {
        _tree = new Octree<TestObject>(0, 0, 0, 10, 1);
        var targetRadius = 1f;
        var rand = new System.Random();
        var inRangeList = new List<TestObject>();
        for (int i = 0; i < 20; i++)
        {
            var newObject = new TestObject();
            inRangeList.Add(newObject);
            var x = MathHelpers.GetRandom(targetRadius / 2f, rand);
            var y = MathHelpers.GetRandom(targetRadius / 2f, rand);
            var z = MathHelpers.GetRandom(targetRadius / 2f, rand);
            _tree.Insert(x, y, z, newObject);
        }
        var outOfRangeList = new List<TestObject>();
        var additionalRange = 10f;
        for (int i = 0; i < 20; i++)
        {
            var newObject = new TestObject();
            outOfRangeList.Add(newObject);
            var x = MathHelpers.GetRandom(targetRadius, rand) + additionalRange;
            var y = MathHelpers.GetRandom(targetRadius, rand) + additionalRange;
            var z = MathHelpers.GetRandom(targetRadius, rand) + additionalRange;

            bool caughtError = false;
            try
            {
                _tree.Insert(x, y, z, newObject);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
               caughtError = true;
            }
            Assert.True(caughtError);
        }

        var objects = _tree.GetRange(0, 0, 0, 10);
        Assert.AreEqual(20, objects.Count());
        for (int i = 0; i < inRangeList.Count; i++)
        {
           Assert.True(objects.Contains(inRangeList[i]));
        }

        for (int i = 0; i < outOfRangeList.Count; i++)
        {
            Assert.False(objects.Contains(outOfRangeList[i]));
        }
    }

    [Test]
    public void GetTwentyInRangeAroundRandomPointWithOneItemPerCell()
    {
        _tree = new Octree<TestObject>(0, 0, 0, 10, 1);

        var initialPointRange = 1f;
        var targetRadius = 3f;
        var rand = new System.Random();
        var inRangeList = new List<TestObject>();

        var startX = MathHelpers.GetRandom(initialPointRange, rand) * MathHelpers.GetPosNeg(rand);
        var startY = MathHelpers.GetRandom(initialPointRange, rand) * MathHelpers.GetPosNeg(rand);
        var startZ = MathHelpers.GetRandom(initialPointRange, rand) * MathHelpers.GetPosNeg(rand);

        for (int i = 0; i < 20; i++)
        {
            var newObject = new TestObject();
            inRangeList.Add(newObject);
            var x = MathHelpers.GetRandom(targetRadius / 2f, rand) * MathHelpers.GetPosNeg(rand) + startX;
            var y = MathHelpers.GetRandom(targetRadius / 2f, rand) * MathHelpers.GetPosNeg(rand) + startY;
            var z = MathHelpers.GetRandom(targetRadius / 2f, rand) * MathHelpers.GetPosNeg(rand) + startZ;
            _tree.Insert(x, y, z, newObject);
        }

        var outOfRangeList = new List<TestObject>();
        var additionalRange = 6f;
        for (int i = 0; i < 20; i++)
        {
            var newObject = new TestObject();
            outOfRangeList.Add(newObject);
            var x = MathHelpers.GetRandom(additionalRange, rand) + targetRadius;
            var y = MathHelpers.GetRandom(additionalRange, rand) + targetRadius;
            var z = MathHelpers.GetRandom(additionalRange, rand) + targetRadius;
            _tree.Insert(x, y, z, newObject);
        }

        var objects = _tree.GetRange(startX, startY, startZ, targetRadius);
        Assert.AreEqual(20, objects.Count(), "Failed to get correct object count around point: x=" + startX + " y=" + startY + " z=" + startZ);

        for (int i = 0; i < inRangeList.Count; i++)
        {
            Assert.True(objects.Contains(inRangeList[i]));
        }

        for (int i = 0; i < outOfRangeList.Count; i++)
        {
            Assert.False(objects.Contains(outOfRangeList[i]));
        }
    }

    [Test]
    public void ObjectsAfterSubDivideExcludeSome3D()
    {
        _tree.Insert(15, 15, 15, _fifteenFifteenObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(3, 3, 3, _threeThreeObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(2, objects.Count);
        Assert.True(objects.Contains(_twoTwoObject));
        Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void RemoveObject3D()
    {
        _tree.Insert(5, 5, 5, _fiveFiveObject);
        var objects = _tree.GetRange(_box).ToList();
        Assert.AreEqual(1, objects.Count());
        Assert.AreEqual(_fiveFiveObject, objects[0]);
        _tree.Remove(_fiveFiveObject);
        objects = _tree.GetRange(_box).ToList();
        Assert.AreEqual(0, objects.Count());
    }

    [Test]
    public void RemoveObjectAfterSubdivide3D()
    {
        _tree.Insert(4, 4, 4, _fourFourObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(3, 3, 3, _threeThreeObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(3, objects.Count);
        Assert.True(objects.Contains(_fourFourObject));
        Assert.True(objects.Contains(_twoTwoObject));
        Assert.True(objects.Contains(_threeThreeObject));
        _tree.Remove(_fourFourObject);
        objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(2, objects.Count);
        Assert.True(objects.Contains(_twoTwoObject));
        Assert.True(objects.Contains(_threeThreeObject));
    }

    [Test]
    public void RemoveCausingSubdivideRemovalThenAdd3D()
    {
        _tree.Insert(4, 4, 4, _fourFourObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(3, 3, 3, _threeThreeObject);
        var objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(3, objects.Count);
        Assert.True(objects.Contains(_fourFourObject));
        Assert.True(objects.Contains(_twoTwoObject));
        Assert.True(objects.Contains(_threeThreeObject));
        _tree.Remove(_fourFourObject);
        _tree.Remove(_twoTwoObject);
        _tree.Remove(_threeThreeObject);
        objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(0, objects.Count);
        _tree.Insert(4, 4, 4, _fourFourObject);
        objects = _tree.GetRange(new ThreeDimensionalBoundingBox(2.5f, 2.5f, 2.5f, 2.5f)).ToList();
        Assert.AreEqual(1, objects.Count);
        Assert.True(objects.Contains(_fourFourObject));
    }

    [Test]
    public void GetSingleObject3D()
    {
        _tree.Insert(4, 4, 4, _fourFourObject);
        var neighbor = _tree.GetNearestObject(2, 2, 2);
        Assert.AreEqual(neighbor, _fourFourObject);
    }

    [Test]
    public void GetNearestObjectOutOfTwo3D()
    {
        _tree.Insert(4, 4, 4, _fourFourObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        var neighbor = _tree.GetNearestObject(5, 5, 5);
        Assert.AreEqual(neighbor, _fourFourObject);
    }

    [Test]
    public void GetNearestObjectOutOfMany3D()
    {
        _tree.Insert(15, 15, 15, _fifteenFifteenObject);
        _tree.Insert(5, 5, 5, _fiveFiveObject);
        _tree.Insert(4, 4, 4, _fourFourObject);
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(3, 3, 3, _threeThreeObject);
        var neighbor = _tree.GetNearestObject(5, 5, 5);
        Assert.AreEqual(neighbor, _fiveFiveObject);
    }

    [Test]
    public void GetNearestWithSeparateOctantMostProximalPoint3D()
    {
        var _nineFourPoint = new TestObject();
        var _sixNinePoint = new TestObject();
        _tree.Insert(2, 2, 2, _twoTwoObject);
        _tree.Insert(6, 9, 15, _sixNinePoint);
        _tree.Insert(9, 4, 15, _nineFourPoint);
        var neighbor = _tree.GetNearestObject(9, 7, 15);
        Assert.AreEqual(_nineFourPoint, neighbor);
    }

    private class TestObject { }
}
