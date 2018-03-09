using Microsoft.VisualStudio.TestTools.UnitTesting;
using nkristek.Stein.Services;
using System;

namespace nkristek.SteinTests
{
    [TestClass]
    public class MsiServiceTests
    {
        private string TestInstallerFilePath
        {
            get
            {
                return null;
            }
        }

        private string TestInstallerCultureTag
        {
            get
            {
                return null;
            }
        }

        private Version TestInstallerVersion
        {
            get
            {
                return null;
            }
        }

        [TestMethod]
        public void TestMsiMethods()
        {
            Assert.IsNotNull(TestInstallerFilePath, "The file path of the test installer is not set.");
            Assert.IsNotNull(TestInstallerCultureTag, "The culture tag of the test installer is not set.");
            Assert.IsNotNull(TestInstallerVersion, "The version of the test installer is not set.");

            // test reading values using file path
            var cultureTag = MsiService.GetCultureTagFromMsi(TestInstallerFilePath);
            Assert.IsNotNull(cultureTag, "Failed reading culture tag.");
            Assert.AreEqual(cultureTag, TestInstallerCultureTag, "Culture tag from msi and expected culture tag aren't equal.");

            var version = MsiService.GetVersionFromMsi(TestInstallerFilePath);
            Assert.IsNotNull(version, "Failed reading version.");
            Assert.AreEqual(version, TestInstallerVersion, "Version from msi and expected version aren't equal.");

            // test reading values using msi database
            using (var database = MsiService.GetMsiDatabase(TestInstallerFilePath))
            {
                var cultureTagFromDatabase = MsiService.GetCultureTagFromMsiDatabase(database);
                Assert.IsNotNull(cultureTagFromDatabase, "Failed reading culture tag using database.");
                Assert.AreEqual(cultureTagFromDatabase, TestInstallerCultureTag, "Culture tag from msi database and expected culture tag aren't equal.");

                var versionFromDatabase = MsiService.GetVersionFromMsiDatabase(database);
                Assert.IsNotNull(versionFromDatabase, "Failed reading version using database.");
                Assert.AreEqual(versionFromDatabase, TestInstallerVersion, "Version from msi database and expected version aren't equal.");
            }
        }
    }
}
