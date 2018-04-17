using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Services.Tests
{
    [TestClass]
    public class InstallServiceTests
    {
        private string TestInstallerFilePath
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
        public void TestInstallMethods()
        {
            TestInstallNonAsyncMethods();
            TestInstallAsyncMethods();
        }
        
        private void TestInstallNonAsyncMethods()
        {
            Assert.IsNotNull(TestInstallerFilePath, "The file path of the test installer is not set.");
            Assert.IsNotNull(TestInstallerProductCode, "The product code of the test installer is not set.");

            var installService = new InstallService();

            // precondition is that the file exists and is not installed
            Assert.IsTrue(File.Exists(TestInstallerFilePath), "The test installer file doesn't exist.");
            installService.RefreshInstalledPrograms();
            Assert.IsFalse(installService.IsProductCodeInstalled(TestInstallerProductCode), "The test installer is already installed. Please uninstall before starting this test.");

            // install
            installService.Install(TestInstallerFilePath);
            installService.RefreshInstalledPrograms();
            Assert.IsTrue(installService.IsProductCodeInstalled(TestInstallerProductCode), "The installation of the test installer failed or checking if it was installed failed.");

            // reinstall
            installService.Reinstall(TestInstallerFilePath);
            installService.RefreshInstalledPrograms();
            Assert.IsTrue(installService.IsProductCodeInstalled(TestInstallerProductCode), "The reinstallation of the test installer failed or checking if it was installed failed.");

            // uninstall
            installService.Uninstall(TestInstallerProductCode);
            installService.RefreshInstalledPrograms();
            Assert.IsFalse(installService.IsProductCodeInstalled(TestInstallerProductCode), "The uninstallation of the test installer failed or checking if it was installed failed.");
        }

        private void TestInstallAsyncMethods()
        {
            Assert.IsFalse(String.IsNullOrEmpty(TestInstallerFilePath), "The file path of the test installer is not set.");
            Assert.IsFalse(String.IsNullOrEmpty(TestInstallerProductCode), "The product code of the test installer is not set.");

            var installService = new InstallService();

            // precondition is that the file exists and is not installed
            Assert.IsTrue(File.Exists(TestInstallerFilePath), "The test installer file doesn't exist.");
            installService.RefreshInstalledPrograms();
            Assert.IsFalse(installService.IsProductCodeInstalled(TestInstallerProductCode), "The test installer is already installed. Please uninstall before starting this test.");

            // install
            installService.InstallAsync(TestInstallerFilePath).Wait();
            installService.RefreshInstalledPrograms();
            Assert.IsTrue(installService.IsProductCodeInstalled(TestInstallerProductCode), "The installation of the test installer failed or checking if it was installed failed.");

            // reinstall
            installService.ReinstallAsync(TestInstallerFilePath).Wait();
            installService.RefreshInstalledPrograms();
            Assert.IsTrue(installService.IsProductCodeInstalled(TestInstallerProductCode), "The reinstallation of the test installer failed or checking if it was installed failed.");

            // uninstall
            installService.UninstallAsync(TestInstallerProductCode).Wait();
            installService.RefreshInstalledPrograms();
            Assert.IsFalse(installService.IsProductCodeInstalled(TestInstallerProductCode), "The uninstallation of the test installer failed or checking if it was installed failed.");
        }
    }
}
