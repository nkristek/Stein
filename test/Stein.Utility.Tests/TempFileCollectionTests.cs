using System;
using System.IO;
using Xunit;

namespace Stein.Utility.Tests
{
    public class TempFileCollectionTests
    {
        [Fact]
        public void Constructor_throws_ArgumentNullException_when_folderPath_empty()
        {
            Assert.Throws<ArgumentNullException>(() => new TempFileCollection(null));
            Assert.Throws<ArgumentNullException>(() => new TempFileCollection(String.Empty));
        }

        [Fact]
        public void CreateUniqueFileName()
        {
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                var first = tempFileCollection.CreateUniqueFileName();
                var second = tempFileCollection.CreateUniqueFileName();
                Assert.NotEqual(first, second);
            }
        }

        [Fact]
        public void CreateUniqueFileName_has_extension()
        {
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                const string fileExtension = "txt";
                var fileName = tempFileCollection.CreateUniqueFileName(fileExtension);
                Assert.EndsWith("." + fileExtension, fileName);
            }
        }

        [Fact]
        public void AddFileName_throws_ArgumentNullException_when_fileName_empty()
        {
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                Assert.Throws<ArgumentNullException>(() => tempFileCollection.AddFileName(null));
                Assert.Throws<ArgumentNullException>(() => tempFileCollection.AddFileName(String.Empty));
            }
        }

        [Fact]
        public void AddFileName_throws_ArgumentException_when_duplicate()
        {
            var fileName = "test";
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                tempFileCollection.AddFileName(fileName);
                Assert.Throws<ArgumentException>(() => tempFileCollection.AddFileName(fileName));
            }
        }

        [Fact]
        public void Dispose_deletes_files()
        {
            var fileName = Path.GetTempFileName();
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            using (File.Create(fileName))
            {
                Assert.True(File.Exists(fileName));
                tempFileCollection.AddFileName(fileName);
            }
            Assert.False(File.Exists(fileName));
        }
    }
}
