using System.Collections.Generic;
using NUnit.Framework;
using BriLib;

namespace UnitTests
{
    [TestFixture]
    [Category("Tuple")]
    public class TupleTests
    {
        Tuple<int, int> a;
        Tuple<int, int> b;
        int aOne = 15;
        int bOne = 20;
        int aTwo = 30;
        int bTwo = 35;

        [SetUp]
        public void Setup()
        {
            a = new Tuple<int, int>(aOne, aTwo);
            b = new Tuple<int, int>(bOne, bTwo);
        }

        [Test]
        public void SameTupleCompare()
        {
            var equalsOutput = a.Equals(a);
            var operatorOutput = a == a;
            Assert.True(equalsOutput, "Same tuple should compare to true");
            Assert.True(operatorOutput, "Same tuple should compare to true");
        }

        [Test]
        public void DifferentTupleCompare()
        {
            var equalsOutput = a.Equals(b);
            var operatorOutput = a == b;
            Assert.False(equalsOutput, "Different tuples with different values should compare to false");
            Assert.False(operatorOutput, "Different tuples with different values should compare to false");
        }

        [Test]
        public void SameComponentsCompare()
        {
            b = new Tuple<int, int>(aOne, aTwo);
            var equalsOutput = a.Equals(b);
            var operatorOutput = a == b;
            Assert.True(equalsOutput, "Different tuples with same values should compare to true");
            Assert.True(operatorOutput, "Different tuples with same values should compare to true");
        }

        [Test]
        public void OneNullOneNotSameData()
        {
            var one = new Tuple<string, string>("Hi", null);
            var two = new Tuple<string, string>("Hi", null);
            var equalsOutput = one.Equals(two);
            var operatorOutput = one == two;
            Assert.True(equalsOutput, "Different tuples with same values should compare to true");
            Assert.True(operatorOutput, "Different tuples with same values should compare to true");
        }

        [Test]
        public void OneNullOneNotDifferentData()
        {
            var one = new Tuple<string, string>("Hi", null);
            var two = new Tuple<string, string>("Hi", "Hi");
            var equalsOutput = one.Equals(two);
            var operatorOutput = one == two;
            Assert.False(equalsOutput, "Different tuples with different values should compare to false");
            Assert.False(operatorOutput, "Different tuples with different values should compare to false");
        }

        [Test]
        public void DictionaryContainment()
        {
            var dict = new Dictionary<Tuple<int, int>, string>();
            dict.Add(a, "Test");
            var newTuple = new Tuple<int, int>(15, 30);
            var contains = dict.ContainsKey(newTuple);
            Assert.True(contains, "Dictionary should report containment of tuple composed of same objects");
            var output = "Test" == dict[a];
            var outputTwo = output == ("Test" == dict[newTuple]);
            Assert.True(output, "Dictionary should allow retrieval using the same tuple key");
            Assert.True(outputTwo, "Dictionary should allow retrieval using the a different tuple with same composition");
        }

        [Test]
        public void DictionaryContainmentFailure()
        {
            var dict = new Dictionary<Tuple<int, int>, string>();
            dict.Add(a, "Test");
            var contains = dict.ContainsKey(b);
            Assert.False(contains, "Dictionary should not report containment of different tuple");
        }

        [Test]
        public void SameTripleCompare()
        {
            var tripleOne = new Tuple<int, int, int>(5, 5, 5);
            var equalsOutput = tripleOne.Equals(tripleOne);
            var operatorOutput = tripleOne == tripleOne;
            Assert.True(equalsOutput, "Same tuple should compare to true");
            Assert.True(operatorOutput, "Same tuple should compare to true");
        }

        [Test]
        public void DifferentTripleCompare()
        {
            var tripleOne = new Tuple<int, int, int>(5, 5, 5);
            var tripleTwo = new Tuple<int, int, int>(5, 1, 5);
            var equalsOutput = tripleOne.Equals(tripleTwo);
            var operatorOutput = tripleOne == tripleTwo;
            Assert.False(equalsOutput, "Different tuples with different values should compare to false");
            Assert.False(operatorOutput, "Different tuples with different values should compare to false");
        }
    }
}
