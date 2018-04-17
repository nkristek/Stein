using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stein.Types.ConfigurationTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stein.Services.Tests
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        private string TestConfigurationFilePath
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
            Assert.IsNotNull(TestConfigurationFilePath, "The file path of the config file is not set.");
            Assert.IsFalse(File.Exists(TestConfigurationFilePath), "The config file already exists.");

            var configurationService = new ConfigurationService(TestConfigurationFilePath);

            configurationService.LoadConfigurationFromDisk();

            configurationService.Configuration.ApplicationFolders = new List<ApplicationFolder>
            {
                new ApplicationFolder()
            };

            configurationService.SaveConfigurationToDisk();

            Assert.IsTrue(File.Exists(TestConfigurationFilePath), "The config file wasn't saved.");

            configurationService.LoadConfigurationFromDisk();

            Assert.IsTrue(configurationService.Configuration.ApplicationFolders.Any(), "Verifying the saved configuration failed.");

            File.Delete(TestConfigurationFilePath);
        }

        private void TestConfigAsync()
        {
            Assert.IsNotNull(TestConfigurationFilePath, "The file path of the config file is not set.");
            Assert.IsFalse(File.Exists(TestConfigurationFilePath), "The config file already exists.");

            var configurationService = new ConfigurationService(TestConfigurationFilePath);

            configurationService.LoadConfigurationFromDiskAsync().Wait();

            configurationService.Configuration.ApplicationFolders = new List<ApplicationFolder>
            {
                new ApplicationFolder()
            };

            configurationService.SaveConfigurationToDiskAsync().Wait();

            Assert.IsTrue(File.Exists(TestConfigurationFilePath), "The config file wasn't saved.");

            configurationService.LoadConfigurationFromDiskAsync().Wait();

            Assert.IsTrue(configurationService.Configuration.ApplicationFolders.Any(), "Verifying the saved configuration failed.");

            File.Delete(TestConfigurationFilePath);
        }
    }
}
