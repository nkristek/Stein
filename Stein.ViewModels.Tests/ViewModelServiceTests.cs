using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stein.Types.ConfigurationTypes;
using Stein.Services.Extensions;
using System;
using System.Linq;
using Stein.Services;

namespace Stein.ViewModels.Tests
{
    [TestClass]
    public class ViewModelServiceTests
    {
        private string TestSetupsFolderPath
        {
            get
            {
                return null;
            }
        }

        [TestMethod]
        public void TestCreateAndUpdateApplicationViewModels()
        {
            Assert.IsNotNull(TestSetupsFolderPath, "The folder path of the test setups is not set.");

            // TODO
            //var applicationFolder = new ApplicationFolder
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Create",
            //    Path = TestSetupsFolderPath,
            //    EnableInstallationLogging = true,
            //    EnableSilentInstallation = true
            //};
            //applicationFolder.SyncWithDisk();
            //Assert.IsTrue(applicationFolder.SubFolders.Any(), "The test setups folder doesn't contain any subfolders.");
            //Assert.IsTrue(applicationFolder.SubFolders.Any(sf => sf.InstallerFiles.Any()), "The test setups folder doesn't contain any subfolders which contain installer files.");

            //// create
            //var applicationViewModel = ViewModelService.CreateViewModel<ApplicationViewModel>(applicationFolder);
            //Assert.IsNotNull(applicationViewModel, "The created ApplicationViewModel is null.");
            //Assert.AreEqual(applicationFolder.Id, applicationViewModel.FolderId, "The ApplicationViewModel wasn't properly created.");
            //Assert.AreEqual(applicationFolder.Name, applicationViewModel.Name, "The ApplicationViewModel wasn't properly created.");
            //Assert.AreEqual(applicationFolder.Path, applicationViewModel.Path, "The ApplicationViewModel wasn't properly created.");
            //Assert.AreEqual(applicationFolder.EnableInstallationLogging, applicationViewModel.EnableInstallationLogging, "The ApplicationViewModel wasn't properly created.");
            //Assert.AreEqual(applicationFolder.EnableSilentInstallation, applicationViewModel.EnableSilentInstallation, "The ApplicationViewModel wasn't properly created.");
            //Assert.IsTrue(applicationViewModel.InstallerBundles.Any(), "The ApplicationViewModel wasn't properly created.");
            //Assert.IsTrue(applicationViewModel.InstallerBundles.Any(ib => ib.Installers.Any()), "The ApplicationViewModel wasn't properly created.");

            //// update
            //applicationFolder.Name = "Update";
            //ViewModelService.UpdateViewModel(applicationViewModel, applicationFolder);
            //Assert.AreEqual(applicationFolder.Name, applicationViewModel.Name, "The ApplicationViewModel wasn't properly updated.");
        }

        [TestMethod]
        public void TestSaveApplicationViewModels()
        {
            Assert.IsNotNull(TestSetupsFolderPath, "The folder path of the test setups is not set.");

            //var applicationFolder = new ApplicationFolder
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Create",
            //    Path = TestSetupsFolderPath,
            //    EnableInstallationLogging = true,
            //    EnableSilentInstallation = true
            //};
            //applicationFolder.SyncWithDisk();
            //Assert.IsTrue(applicationFolder.SubFolders.Any(), "The test setups folder doesn't contain any subfolders.");
            //Assert.IsTrue(applicationFolder.SubFolders.Any(sf => sf.InstallerFiles.Any()), "The test setups folder doesn't contain any subfolders which contain installer files.");

            //var applicationViewModel = ViewModelService.CreateViewModel<ApplicationViewModel>(applicationFolder);
            //applicationViewModel.Name = "Save";

            //ViewModelService.SaveViewModel(applicationViewModel, applicationFolder);
            //Assert.AreEqual(applicationViewModel.Name, applicationFolder.Name, "The ApplicationViewModel wasn't properly saved.");
        }
    }
}
