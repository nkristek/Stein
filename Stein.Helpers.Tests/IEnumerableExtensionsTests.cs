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
