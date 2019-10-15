using System;
using System.IO;
using Xunit;

namespace Stein.Utility.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("12", new[] { '1' }, true)]
        [InlineData("12", new[] { '3' }, false)]
        public void Contains(string testData, char[] charsToCheck, bool expectedResult)
        {
            Assert.Equal(expectedResult, testData.Contains(charsToCheck));
        }

        [Theory]
        [InlineData(null, new[] { '1' }, null)]
        [InlineData("", new[] { '1' }, "")]
        [InlineData("12", new[] { '1' }, "2")]
        [InlineData("12", new[] { '3' }, "12")]
        [InlineData("12", new[] { '1', '2' }, "")]
        public void Remove(string testData, char[] charsToRemove, string expectedResult)
        {
            Assert.Equal(expectedResult, testData.Remove(charsToRemove));
        }

        [Theory]
        [InlineData(null, new[] { '1' }, "2", null)]
        [InlineData("", new[] { '1' }, "2", "")]
        [InlineData("12", new[] { '1' }, "2", "22")]
        [InlineData("12", new[] { '3' }, "2", "12")]
        [InlineData("12", new[] { '1', '2' }, "3", "33")]
        public void Replace(string testData, char[] charsToReplace, string replacement, string expectedResult)
        {
            Assert.Equal(expectedResult, testData.Replace(replacement, charsToReplace));
        }
    }
}
