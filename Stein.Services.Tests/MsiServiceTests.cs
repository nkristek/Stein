using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stein.Services.Types;

namespace Stein.Services.Tests
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

        private string TestInstallerName
        {
            get
            {
                return null;
            }
        }

        private string TestInstallerProductCode
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
            Assert.IsNotNull(TestInstallerName, "The name of the test installer is not set.");
            Assert.IsNotNull(TestInstallerProductCode, "The productcode of the test installer is not set.");

            var msiService = new MsiService();

            // test reading values using file path
            var name = msiService.GetPropertyFromMsi(TestInstallerFilePath, MsiPropertyName.ProductName);
            Assert.IsNotNull(name, "Failed reading name.");
            Assert.AreEqual(name, TestInstallerName, "Name from msi and expected name aren't equal.");

            var productCode = msiService.GetPropertyFromMsi(TestInstallerFilePath, MsiPropertyName.ProductCode);
            Assert.IsNotNull(productCode, "Failed reading productcode.");
            Assert.AreEqual(productCode, TestInstallerProductCode, "Version from msi and expected version aren't equal.");

            // test reading values using msi database
            using (var database = msiService.GetMsiDatabase(TestInstallerFilePath))
            {
                var nameFromDatabase = msiService.GetPropertyFromMsiDatabase(database, MsiPropertyName.ProductName);
                Assert.IsNotNull(nameFromDatabase, "Failed reading name using database.");
                Assert.AreEqual(nameFromDatabase, TestInstallerName, "Name from msi database and expected name aren't equal.");

                var productCodeFromDatabase = msiService.GetPropertyFromMsiDatabase(database, MsiPropertyName.ProductCode);
                Assert.IsNotNull(productCodeFromDatabase, "Failed reading productcode using database.");
                Assert.AreEqual(productCodeFromDatabase, TestInstallerProductCode, "Productcode from msi database and expected productcode aren't equal.");
            }
        }
    }
}
