using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Helpers.Tests
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void TestIEnumerableForEach()
        {
            var testInput = new[] { new object(), new object(), new object() };
            var result = 0;
            testInput.ForEach(i => result++);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestDistinctBy()
        {
            var first = new KeyValuePair<int, string>(1, "test");
            var second = new KeyValuePair<int, string>(2, "test");
            var third = new KeyValuePair<int, string>(3, "test2");
            var sequence = new List<KeyValuePair<int, string>> { first, second, third };
            var expectedDistinctValues = new List<KeyValuePair<int, string>> { first, third };
            var actualDistinctValues = sequence.DistinctBy(kvp => kvp.Value);
            Assert.IsTrue(expectedDistinctValues.SequenceEqual(actualDistinctValues));
        }

        [TestMethod]
        public void TestMergeSequence()
        {
            var firstSequence = new List<string> { "1", "3" };
            var secondSequence = new List<string> { "1", "2", "3" };
            var mergedSequence = firstSequence.MergeSequence(secondSequence);
            Assert.IsTrue(mergedSequence.SequenceEqual(new List<string> { "1", "2", "3" }));

            firstSequence = new List<string> { "1", "2", "4" };
            secondSequence = new List<string> { "2", "3", "4", "5" };
            mergedSequence = firstSequence.MergeSequence(secondSequence);
            Assert.IsTrue(mergedSequence.SequenceEqual(new List<string> { "1", "2", "3", "4", "5" }));

            firstSequence = new List<string> { "1", "2", "3" };
            secondSequence = new List<string> { "4", "3", "2", "1" };
            mergedSequence = firstSequence.MergeSequence(secondSequence);
            Assert.IsTrue(mergedSequence.SequenceEqual(new List<string> { "1", "2", "4", "3" }));
        }
    }
}
