using System;
using System.IO;
using Xunit;

namespace Stein.Helpers.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ContainsInvalidPathChars()
        {
            var invalidPath = String.Concat(Path.GetInvalidPathChars());
            Assert.True(invalidPath.ContainsInvalidPathChars());
            Assert.False("test".ContainsInvalidPathChars());
        }

        [Fact]
        public void ReplaceInvalidPathChars()
        {
            var invalidPath = String.Concat(Path.GetInvalidPathChars());
            var validPath = invalidPath.ReplaceInvalidPathChars('a');
            Assert.False(validPath.ContainsInvalidPathChars());
        }

        [Fact]
        public void ContainsInvalidFileNameChars()
        {
            var invalidFileName = String.Concat(Path.GetInvalidFileNameChars());
            Assert.True(invalidFileName.ContainsInvalidFileNameChars());
            Assert.False("test".ContainsInvalidFileNameChars());
        }

        [Fact]
        public void ReplaceInvalidFileNameChars()
        {
            var invalidFileName = String.Concat(Path.GetInvalidFileNameChars());
            var validFileName = invalidFileName.ReplaceInvalidFileNameChars('a');
            Assert.False(validFileName.ContainsInvalidFileNameChars());
        }
    }
}
