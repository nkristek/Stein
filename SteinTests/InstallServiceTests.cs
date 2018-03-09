using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nkristek.Stein.Services;

namespace nkristek.SteinTests
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
            Assert.IsNotNull(TestInstallerFilePath, "The file path of the test installer is not set.");
            Assert.IsNotNull(TestInstallerProductCode, "The product code of the test installer is not set.");

            // precondition is that the file exists and is not installed
            Assert.IsTrue(File.Exists(TestInstallerFilePath), "The test installer file doesn't exist.");
            InstallService.RefreshInstalledPrograms();
            Assert.IsFalse(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The test installer is already installed. Please uninstall before starting this test.");

            // install
            InstallService.Install(TestInstallerFilePath);
            InstallService.RefreshInstalledPrograms();
            Assert.IsTrue(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The installation of the test installer failed or checking if it was installed failed.");

            // reinstall
            InstallService.Reinstall(TestInstallerFilePath);
            InstallService.RefreshInstalledPrograms();
            Assert.IsTrue(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The reinstallation of the test installer failed or checking if it was installed failed.");

            // uninstall
            InstallService.Uninstall(TestInstallerProductCode);
            InstallService.RefreshInstalledPrograms();
            Assert.IsFalse(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The uninstallation of the test installer failed or checking if it was installed failed.");
        }

        [TestMethod]
        public void TestInstallAsyncMethods()
        {
            Assert.IsFalse(String.IsNullOrEmpty(TestInstallerFilePath), "The file path of the test installer is not set.");
            Assert.IsFalse(String.IsNullOrEmpty(TestInstallerProductCode), "The product code of the test installer is not set.");

            // precondition is that the file exists and is not installed
            Assert.IsTrue(File.Exists(TestInstallerFilePath), "The test installer file doesn't exist.");
            InstallService.RefreshInstalledProgramsAsync().Wait();
            Assert.IsFalse(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The test installer is already installed. Please uninstall before starting this test.");

            // install
            InstallService.InstallAsync(TestInstallerFilePath).Wait();
            InstallService.RefreshInstalledProgramsAsync().Wait();
            Assert.IsTrue(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The installation of the test installer failed or checking if it was installed failed.");

            // reinstall
            InstallService.ReinstallAsync(TestInstallerFilePath).Wait();
            InstallService.RefreshInstalledProgramsAsync().Wait();
            Assert.IsTrue(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The reinstallation of the test installer failed or checking if it was installed failed.");

            // uninstall
            InstallService.UninstallAsync(TestInstallerProductCode).Wait();
            InstallService.RefreshInstalledProgramsAsync().Wait();
            Assert.IsFalse(InstallService.IsProductCodeInstalled(TestInstallerProductCode), "The uninstallation of the test installer failed or checking if it was installed failed.");
        }
    }
}
