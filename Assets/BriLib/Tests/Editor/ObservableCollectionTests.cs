using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BriLib;

[TestFixture]
public class ObservableCollectionTests
{
    private ObservableCollection<TestObject> _list;
    private TestObject _objOne;
    private TestObject _objTwo;
    private TestObject _objThree;

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

    }

    [Test]
    public void MembersPresent()
    {

    }

    [Test]
    public void RemoveItem()
    {

    }

    [Test]
    public void UpdateItem()
    {

    }

    [Test]
    public void ClearItem()
    {

    }

    [Test]
    public void GetMappedCollection()
    {

    }

    [Test]
    public void MappedCollectionMembersPresent()
    {

    }

    [Test]
    public void MappedCollectionUnderlyingAdd()
    {

    }

    [Test]
    public void MappedCollectionUnderlyingRemove()
    {

    }

    [Test]
    public void MappedCollUnderlyingReplace()
    {

    }

    [Test]
    public void MappedCollUnderlyingClear()
    {

    }

    [Test]
    public void ReduceExecute()
    {

    }

    [Test]
    public void GetFilteredCollection()
    {

    }

    [Test]
    public void FilteredCollMembersPresent()
    {

    }

    [Test]
    public void FilteredCollAddMember()
    {

    }

    [Test]
    public void FilteredCollAddFilteredOutMember()
    {

    }

    [Test]
    public void FilteredCollReplaceMember()
    {

    }

    [Test]
    public void FilteredCollReplaceNonpresentMember()
    {

    }

    [Test]
    public void FilteredCollReplacePresentMemberWithFilteredOutMember()
    {

    }

    [Test]
    public void FilteredCollReplaceNonPresentWithNonPresent()
    {

    }

    [Test]
    public void FilteredCollRemoveMember()
    {

    }

    [Test]
    public void FilteredRemoveNonPresent()
    {

    }

    [Test]
    public void FilteredClear()
    {

    }

    [Test]
    public void GetSortedCollection()
    {

    }

    [Test]
    public void GetSortedMemberOrder()
    {

    }

    [Test]
    public void SortedAddMember()
    {

    }

    [Test]
    public void SortedRemoveMember()
    {

    }

    [Test]
    public void SortedReplace()
    {

    }

    [Test]
    public void SortedClear()
    {

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
