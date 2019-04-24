using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stein.Utility.Tests
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void ForEach()
        {
            var testInput = new[] { new object(), new object(), new object() };
            var result = 0;
            testInput.ForEach(i => result++);
            Assert.Equal(3, result);
        }

        [Fact]
        public void DistinctBy()
        {
            var first = new KeyValuePair<int, string>(1, "test");
            var second = new KeyValuePair<int, string>(2, "test");
            var third = new KeyValuePair<int, string>(3, "test2");
            var sequence = new List<KeyValuePair<int, string>> { first, second, third };
            var expectedDistinctValues = new List<KeyValuePair<int, string>> { first, third };
            var actualDistinctValues = sequence.DistinctBy(kvp => kvp.Value);
            Assert.Equal(expectedDistinctValues, actualDistinctValues);
        }

        [Fact]
        public void MergeSequence()
        {
            var firstSequence = new List<string> { "1", "3" };
            var secondSequence = new List<string> { "1", "2", "3" };
            var mergedSequence = firstSequence.MergeSequence(secondSequence);
            Assert.True(mergedSequence.SequenceEqual(new List<string> { "1", "2", "3" }));

            firstSequence = new List<string> { "1", "2", "4" };
            secondSequence = new List<string> { "2", "3", "4", "5" };
            mergedSequence = firstSequence.MergeSequence(secondSequence);
            Assert.True(mergedSequence.SequenceEqual(new List<string> { "1", "2", "3", "4", "5" }));

            firstSequence = new List<string> { "1", "2", "3" };
            secondSequence = new List<string> { "4", "3", "2", "1" };
            mergedSequence = firstSequence.MergeSequence(secondSequence);
            Assert.True(mergedSequence.SequenceEqual(new List<string> { "1", "2", "4", "3" }));
        }

        [Fact]
        public void SequenceEqual()
        {
            IEnumerable<string> firstSequence = new List<string> { "1" };
            IEnumerable<string> secondSequence = new List<string> { "1" };
            Assert.True(firstSequence.SequenceEqual(secondSequence, (f, s) => f == s));

            firstSequence = new List<string> { "1" };
            secondSequence = new List<string> { "2" };
            Assert.False(firstSequence.SequenceEqual(secondSequence, (f, s) => f == s));

            firstSequence = Enumerable.Empty<string>();
            secondSequence = Enumerable.Empty<string>();
            Assert.True(firstSequence.SequenceEqual(secondSequence, (f, s) => f == s));

            firstSequence = new List<string> { "1" };
            secondSequence = new List<string> { "1", "2" };
            Assert.False(firstSequence.SequenceEqual(secondSequence, (f, s) => f == s));

            firstSequence = new List<string> { "1", "2" };
            secondSequence = new List<string> { "1" };
            Assert.False(firstSequence.SequenceEqual(secondSequence, (f, s) => f == s));
        }
    }
}
