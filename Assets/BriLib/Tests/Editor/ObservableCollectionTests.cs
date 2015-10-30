using System;
using NUnit.Framework;
using BriLib;

[TestFixture]
public class ObservableCollectionTests
{
    private ObservableCollection<TestObject> _list;
    private TestObject _objOne;
    private TestObject _objTwo;
    private TestObject _objThree;

    private object _addedObj;
    private int _addedObjIndex;

    [SetUp]
    public void Setup()
    {
        _list = new ObservableCollection<TestObject>();
        _objOne = new TestObject { Value = 1 };
        _objTwo = new TestObject { Value = 2 };
        _objThree = new TestObject { Value = 3 };
    }

    [Test]
    public void AddItem()
    {
        TestObject addedItem = null;
        int index = -1;
        _list.OnAdded += (ind, obj) => { index = ind; addedItem = obj; };
        _list.Add(_objOne);
        Assert.AreEqual(0, index, "Item added to index 0");
        Assert.AreEqual(_objOne, addedItem, "Added item was item one");
    }

    [Test]
    public void MembersPresent()
    {
        Assert.AreEqual(0, _list.Count, "Count should still be 0");
        _list.Add(_objOne);
        Assert.AreEqual(1, _list.Count, "Count should be 1");
        Assert.True(_list.Contains(_objOne), "Object one should be in list");
        _list.Add(_objTwo);
        Assert.AreEqual(2, _list.Count, "Count should become 2");
        Assert.True(_list.Contains(_objTwo), "Object two should be in list");
        Assert.True(_list.Contains(_objOne), "Object one should be in list");
    }

    [Test]
    public void RemoveItem()
    {
        TestObject removedItem = null;
        int index = -1;
        _list.Add(_objTwo);
        _list.Add(_objOne);
        _list.Add(_objThree);
        _list.OnRemoved += (ind, obj) => { index = ind; removedItem = obj; };
        Assert.True(_list.Contains(_objOne), "Object one still in list");
        Assert.AreEqual(3, _list.Count, "Count should be three");
        _list.Remove(_objOne);
        Assert.AreEqual(1, index, "Object should be removed from index 1");
        Assert.AreEqual(removedItem, _objOne, "Object one should be removed");
    }

    [Test]
    public void UpdateItem()
    {
        var index = -1;
        TestObject obj = null;
        _list.Add(_objOne);
        _list.Add(_objTwo);
        _list.OnReplaced += (ind, old, newObj) => { index = ind; obj = newObj; };
        _list[1] = _objThree;
        Assert.AreEqual(1, index, "Index replaced should be 1");
        Assert.AreEqual(obj, _objThree, "Object two should be replaced with obj three");
    }

    [Test]
    public void ClearItem()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        _list.Add(_objThree);
        Assert.AreEqual(3, _list.Count, "Count should be three");
        _list.Clear();
        Assert.AreEqual(0, _list.Count, "Count should be 0");
        Assert.False(_list.Contains(_objTwo), "Object two should be removed");
        Assert.False(_list.Contains(_objThree), "Object three should be removed");
        Assert.False(_list.Contains(_objOne), "Object one should be removed");
    }

    [Test]
    public void GetMappedCollection()
    {
        Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        _list.Add(_objThree);
        var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
        Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
        bool hadOne = false;
        bool hadTwo = false;
        bool hadThree = false;
        mappedList.ForEach(entry =>
        {
            hadOne |= entry.Object == _objOne;
            hadTwo |= entry.Object == _objTwo;
            hadThree |= entry.Object == _objThree;
        });

        Assert.True(hadOne, "Mapped list should have contained item one");
        Assert.True(hadTwo, "Mapped list should have contained item two");
        Assert.True(hadThree, "Mapped list should have contained item three");
    }

    [Test]
    public void MappedCollectionUnderlyingAdd()
    {
        Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
        Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
        WrappedTestObject test = null;
        int testIndex = -1;
        mappedList.OnAdded += (ind, item) => { testIndex = ind; test = item; };
        _list.Add(_objThree);
        Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
        Assert.AreEqual(2, testIndex, "New entry should be at index 2");
        Assert.AreEqual(_objThree, test.Object, "Object three should have been added");
    }

    [Test]
    public void MappedCollectionUnderlyingRemove()
    {
        Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        _list.Add(_objThree);
        var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
        Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
        WrappedTestObject test = null;
        int testIndex = -1;
        mappedList.OnRemoved += (ind, item) => { testIndex = ind; test = item; };
        _list.Remove(_objTwo);
        Assert.AreEqual(1, testIndex, "Removed entry should be at index 1");
        Assert.AreEqual(_objTwo, test.Object, "Object two should have been removeed");
        Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
    }

    [Test]
    public void MappedCollUnderlyingReplace()
    {
        Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
        Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
        WrappedTestObject newObj = null;
        WrappedTestObject oldObj = null;
        int testIndex = -1;
        mappedList.OnReplaced += (ind, item, itemTwo) => { testIndex = ind; oldObj = item; newObj = itemTwo; };
        _list[1] = _objThree;
        Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
        Assert.AreEqual(1, testIndex, "New entry should be at index 1");
        Assert.AreEqual(_objThree, newObj.Object, "Object three should have been added");
        Assert.AreEqual(_objTwo, oldObj.Object, "Object two should have been swapped out");
    }

    [Test]
    public void MappedCollUnderlyingClear()
    {
        Func<TestObject, WrappedTestObject> map = (entry) => { return new WrappedTestObject { Object = entry }; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var mappedList = _list.Map(map) as ObservableCollection<WrappedTestObject>;
        Assert.AreEqual(2, mappedList.Count, "Mapped list should have 2 entries");
        bool clearCalled = true;
        mappedList.OnCleared += () => { clearCalled = true; };
        _list.Clear();
        Assert.AreEqual(0, mappedList.Count, "Mapped list should have 0 entries");
        Assert.True(clearCalled, "Clear should have been called");
    }

    [Test]
    public void GetMappedNonGeneric()
    {
        Func<object, object> map = (entry) => { return new WrappedTestObject { Object = (TestObject)entry }; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        _list.Add(_objThree);
        var mappedList = _list.Map(map);
        Assert.AreEqual(3, mappedList.Count, "Mapped list should have 3 entries");
        bool hadOne = false;
        bool hadTwo = false;
        bool hadThree = false;
        mappedList.ForEach(entry =>
        {
            hadOne |= (entry as WrappedTestObject).Object == _objOne;
            hadTwo |= (entry as WrappedTestObject).Object == _objTwo;
            hadThree |= (entry as WrappedTestObject).Object == _objThree;
        });

        Assert.True(hadOne, "Mapped list should have contained item one");
        Assert.True(hadTwo, "Mapped list should have contained item two");
        Assert.True(hadThree, "Mapped list should have contained item three");
    }

    [Test]
    public void ReduceExecute()
    {
        Func<TestObject, int, int> add = (obj, index) => { return index + obj.Value; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        int start = _list.Reduce(0, add);
        Assert.AreEqual(3, start, "Seed value should have incremented to 3");
    }

    [Test]
    public void ReduceNonGeneric()
    {
        Func<object, int, int> add = (obj, index) => { return index + (obj as TestObject).Value; };
        _list.Add(_objOne);
        _list.Add(_objTwo);
        int start = _list.ReduceNonGeneric(0, add);
        Assert.AreEqual(3, start, "Seed value should have incremented to 3");
    }

    [Test]
    public void GetFilteredCollection()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var filtered = _list.Filter((obj) => { return obj.Value == 1; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
    }

    [Test]
    public void FilteredCollNonGeneric()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var filtered = _list.FilterNonGeneric((obj) => { return (obj as TestObject).Value == 1; });
        Assert.AreEqual(1, filtered.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, filtered[0], "Object at position 0 should be objOne");
    }

    [Test]
    public void FilteredCollAddMember()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        _list.Add(_objThree);
        Assert.AreEqual(2, cast.Count, "Filtered list should have additional object");
        Assert.AreEqual(_objThree, cast[1], "Object three should be at position 1");
    }

    [Test]
    public void FilteredCollAddFilteredOutMember()
    {
        _list.Add(_objOne);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        _list.Add(_objTwo);
        Assert.AreEqual(1, cast.Count, "Filtered list should not have additional object");
        Assert.AreEqual(_objOne, cast[0], "Object one should be at position 0");
    }

    [Test]
    public void FilteredCollReplaceMember()
    {
        _list.Add(_objOne);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        _list[0] = _objThree;
        Assert.AreEqual(1, cast.Count, "Filtered list should not have additional object");
        Assert.AreEqual(_objThree, cast[0], "Object three should be at position 0");
    }

    [Test]
    public void FilteredCollReplaceNonpresentMember()
    {
        _list.Add(_objTwo);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(0, cast.Count, "Filtered list should be empty");
        _list[0] = _objThree;
        Assert.AreEqual(1, cast.Count, "Filtered list should not have additional object");
        Assert.AreEqual(_objThree, cast[0], "Object three should be at position 0");
    }

    [Test]
    public void FilteredCollReplacePresentMemberWithFilteredOutMember()
    {
        _list.Add(_objOne);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        _list[0] = _objTwo;
        Assert.AreEqual(0, cast.Count, "Filtered list should not have any objects");
    }

    [Test]
    public void FilteredCollReplaceNonPresentWithNonPresent()
    {
        _list.Add(_objTwo);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(0, cast.Count, "Filtered list should be empty");
        _list[0] = new TestObject() { Value = 4 };
        Assert.AreEqual(0, cast.Count, "Filtered list should not have any objects");
    }

    [Test]
    public void FilteredCollRemoveMember()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        _list.Remove(_objOne);
        Assert.AreEqual(0, cast.Count, "Filtered list should have no objects");
    }

    [Test]
    public void FilteredRemoveNonPresent()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(1, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        _list.Remove(_objTwo);
        Assert.AreEqual(1, cast.Count, "Filtered list should have one object");
        Assert.AreEqual(_objOne, cast[0], "Object one should be at position 0");
    }

    [Test]
    public void FilteredClear()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        _list.Add(_objThree);
        var filtered = _list.Filter((obj) => { return obj.Value == 1 || obj.Value == 3; });
        var cast = filtered as ObservableCollection<TestObject>;
        Assert.AreEqual(2, cast.Count, "Filtered list should have just one object");
        Assert.AreEqual(_objOne, cast[0], "Object at position 0 should be objOne");
        Assert.AreEqual(_objThree, cast[1], "Object three should be at position 1");
        bool clearCalled = false;
        filtered.OnCleared += () => clearCalled = true;
        _list.Clear();
        Assert.True(clearCalled, "Clear callback should have triggered");
        Assert.AreEqual(0, cast.Count, "Filtered list should become empty");
    }

    [Test]
    public void GetSortedCollection()
    {
        _list.Add(_objThree);
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var sorted = _list.Sort((obj, obj2) => { return obj.Value - obj2.Value; });
        var cast = sorted as ObservableCollection<TestObject>;
        Assert.AreEqual(3, cast.Count, "Sorted list count should be 3");
        Assert.AreEqual(_objOne, cast[0], "Object one should be at position 0");
        Assert.AreEqual(_objTwo, cast[1], "Object two should be at position 1");
        Assert.AreEqual(_objThree, cast[2], "Object three should be at position 2");
    }

    [Test]
    public void GetSortedGeneric()
    {
        _list.Add(_objThree);
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
        Assert.AreEqual(3, sorted.Count, "Sorted list count should be 3");
        Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
        Assert.AreEqual(_objTwo, sorted[1], "Object two should be at position 1");
        Assert.AreEqual(_objThree, sorted[2], "Object three should be at position 2");
    }

    [Test]
    public void SortedAddMember()
    {
        _list.Add(_objThree);
        _list.Add(_objTwo);
        var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
        Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
        Assert.AreEqual(_objTwo, sorted[0], "Object two should be at position 1");
        Assert.AreEqual(_objThree, sorted[1], "Object three should be at position 2");
        _list.Add(_objOne);
        Assert.AreEqual(3, sorted.Count, "Sorted list count should be 3");
        Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
    }

    [Test]
    public void SortedRemoveMember()
    {
        _list.Add(_objThree);
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
        Assert.AreEqual(3, sorted.Count, "Sorted list count should be 3");
        Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
        Assert.AreEqual(_objTwo, sorted[1], "Object two should be at position 1");
        Assert.AreEqual(_objThree, sorted[2], "Object three should be at position 2");
        _list.Remove(_objTwo);
        Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
        Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
        Assert.AreEqual(_objThree, sorted[1], "Object three should be at position 1");
    }

    [Test]
    public void SortedReplace()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
        Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
        Assert.AreEqual(_objOne, sorted[0], "Object one should be at position 0");
        Assert.AreEqual(_objTwo, sorted[1], "Object two should be at position 1");
        _list[0] = _objThree;
        Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
        Assert.AreEqual(_objTwo, sorted[0], "Object two should be at position 0");
        Assert.AreEqual(_objThree, sorted[1], "Object three should be at position 1");
    }

    [Test]
    public void SortedClear()
    {
        _list.Add(_objOne);
        _list.Add(_objTwo);
        var sorted = _list.SortNonGeneric((obj, obj2) => { return (obj as TestObject).Value - (obj2 as TestObject).Value; });
        Assert.AreEqual(2, sorted.Count, "Sorted list count should be 2");
        bool clearCalled = false;
        sorted.OnCleared += () => clearCalled = true;
        _list.Clear();
        Assert.True(clearCalled, "Clear should have been called");
        Assert.AreEqual(0, sorted.Count, "Sorted list should now be empty");
    }

    [Test]
    public void GetUnion()
    {

    }

    [Test]
    public void UnionCollMembers()
    {

    }

    [Test]
    public void UnionAddLeft()
    {

    }

    [Test]
    public void UnionAddRight()
    {

    }

    [Test]
    public void UnionRemoveLeft()
    {

    }

    [Test]
    public void UnionRemoveRight()
    {

    }

    [Test]
    public void UnionReplaceLeft()
    {

    }

    [Test]
    public void UnionReplaceRight()
    {

    }

    [Test]
    public void UnionClearLeft()
    {

    }

    [Test]
    public void UnionClearRight()
    {

    }

    private class WrappedTestObject
    {
        public TestObject Object;
    }

    private class TestObject
    {
        public int Value;
    }
}
