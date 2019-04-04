using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Helpers.Tests
{
    [TestClass]
    public class TempFileCollectionTests
    {
        [TestMethod]
        public void Constructor_throws_ArgumentNullException_when_folderPath_empty()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TempFileCollection(null));
            Assert.ThrowsException<ArgumentNullException>(() => new TempFileCollection(String.Empty));
        }
        
        [TestMethod]
        public void CreateUniqueFileName()
        {
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                var first = tempFileCollection.CreateUniqueFileName();
                var second = tempFileCollection.CreateUniqueFileName();
                Assert.AreNotEqual(first, second);
            }
        }

        [TestMethod]
        public void CreateUniqueFileName_has_extension()
        {
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                const string fileExtension = "txt";
                var fileName = tempFileCollection.CreateUniqueFileName(fileExtension);
                Assert.IsTrue(fileName.EndsWith("." + fileExtension));
            }
        }

        [TestMethod]
        public void AddFileName_throws_ArgumentNullException_when_fileName_empty()
        {
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                Assert.ThrowsException<ArgumentNullException>(() => tempFileCollection.AddFileName(null));
                Assert.ThrowsException<ArgumentNullException>(() => tempFileCollection.AddFileName(String.Empty));
            }
        }

        [TestMethod]
        public void AddFileName_throws_ArgumentException_when_duplicate()
        {
            var fileName = "test";
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            {
                tempFileCollection.AddFileName(fileName);
                Assert.ThrowsException<ArgumentException>(() => tempFileCollection.AddFileName(fileName));
            }
        }

        [TestMethod]
        public void Dispose_deletes_files()
        {
            var fileName = Path.GetTempFileName();
            using (var tempFileCollection = new TempFileCollection(Path.GetTempPath()))
            using (File.Create(fileName))
            {
                Assert.IsTrue(File.Exists(fileName));
                tempFileCollection.AddFileName(fileName);
            }
            Assert.IsFalse(File.Exists(fileName));
        }
    }
}
