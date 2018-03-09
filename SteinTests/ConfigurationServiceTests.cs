using Microsoft.VisualStudio.TestTools.UnitTesting;
using nkristek.Stein.ConfigurationTypes;
using nkristek.Stein.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nkristek.SteinTests
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        private string TestConfigurationFolderPath
        {
            get
            {
                return null;
            }
        }

        [TestMethod]
        public void TestConfig()
        {
            TestConfigNonAsync();
            TestConfigAsync();
        }

        private void TestConfigNonAsync()
        {
            Assert.IsNotNull(TestConfigurationFolderPath, "The folder path of the config folder is not set.");

            ConfigurationService.ConfiguationFolderPath = TestConfigurationFolderPath;
            Assert.IsFalse(File.Exists(ConfigurationService.ConfiguationPath), "The config file already exists.");

            ConfigurationService.LoadConfigurationFromDisk();

            ConfigurationService.Configuration.ApplicationFolders = new List<ApplicationFolder>
            {
                new ApplicationFolder()
            };

            ConfigurationService.SaveConfigurationToDisk();

            Assert.IsTrue(File.Exists(ConfigurationService.ConfiguationPath), "The config file wasn't saved.");

            ConfigurationService.LoadConfigurationFromDisk();

            Assert.IsTrue(ConfigurationService.Configuration.ApplicationFolders.Any(), "Verifying the saved configuration failed.");

            File.Delete(ConfigurationService.ConfiguationPath);
        }

        private void TestConfigAsync()
        {
            Assert.IsNotNull(TestConfigurationFolderPath, "The folder path of the config folder is not set.");

            ConfigurationService.ConfiguationFolderPath = TestConfigurationFolderPath;
            Assert.IsFalse(File.Exists(ConfigurationService.ConfiguationPath), "The config file already exists.");

            ConfigurationService.LoadConfigurationFromDiskAsync().Wait();

            ConfigurationService.Configuration.ApplicationFolders = new List<ApplicationFolder>
            {
                new ApplicationFolder()
            };

            ConfigurationService.SaveConfigurationToDiskAsync().Wait();

            Assert.IsTrue(File.Exists(ConfigurationService.ConfiguationPath), "The config file wasn't saved.");

            ConfigurationService.LoadConfigurationFromDiskAsync().Wait();

            Assert.IsTrue(ConfigurationService.Configuration.ApplicationFolders.Any(), "Verifying the saved configuration failed.");

            File.Delete(ConfigurationService.ConfiguationPath);
        }
    }
}
