using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Helpers.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void TestContainsInvalidPathChars()
        {
            var invalidPath = String.Concat(Path.GetInvalidPathChars());
            Assert.IsTrue(invalidPath.ContainsInvalidPathChars());
            Assert.IsFalse("test".ContainsInvalidPathChars());
        }

        [TestMethod]
        public void TestReplaceInvalidPathChars()
        {
            var invalidPath = String.Concat(Path.GetInvalidPathChars());
            var validPath = invalidPath.ReplaceInvalidPathChars('a');
            Assert.IsFalse(validPath.ContainsInvalidPathChars());
        }

        [TestMethod]
        public void TestContainsInvalidFileNameChars()
        {
            var invalidFileName = String.Concat(Path.GetInvalidFileNameChars());
            Assert.IsTrue(invalidFileName.ContainsInvalidFileNameChars());
            Assert.IsFalse("test".ContainsInvalidFileNameChars());
        }

        [TestMethod]
        public void TestReplaceInvalidFileNameChars()
        {
            var invalidFileName = String.Concat(Path.GetInvalidFileNameChars());
            var validFileName = invalidFileName.ReplaceInvalidFileNameChars('a');
            Assert.IsFalse(validFileName.ContainsInvalidFileNameChars());
        }
    }
}
