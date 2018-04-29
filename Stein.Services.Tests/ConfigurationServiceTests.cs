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
        private static string TestConfigurationFilePath => null;

        [TestMethod]
        public void TestConfig()
        {
            TestConfigNonAsync();
            TestConfigAsync();
        }

        private static void TestConfigNonAsync()
        {
            Assert.IsNotNull(TestConfigurationFilePath, "The file path of the config file is not set.");
            Assert.IsFalse(File.Exists(TestConfigurationFilePath), "The config file already exists.");

            var configurationService = new ConfigurationService(TestConfigurationFilePath);

            configurationService.LoadConfiguration();

            configurationService.Configuration.ApplicationFolders = new List<ApplicationFolder>
            {
                new ApplicationFolder()
            };

            configurationService.SaveConfiguration();

            Assert.IsTrue(File.Exists(TestConfigurationFilePath), "The config file wasn't saved.");

            configurationService.LoadConfiguration();

            Assert.IsTrue(configurationService.Configuration.ApplicationFolders.Any(), "Verifying the saved configuration failed.");

            File.Delete(TestConfigurationFilePath);
        }

        private static void TestConfigAsync()
        {
            Assert.IsNotNull(TestConfigurationFilePath, "The file path of the config file is not set.");
            Assert.IsFalse(File.Exists(TestConfigurationFilePath), "The config file already exists.");

            var configurationService = new ConfigurationService(TestConfigurationFilePath);

            configurationService.LoadConfigurationAsync().Wait();

            configurationService.Configuration.ApplicationFolders = new List<ApplicationFolder>
            {
                new ApplicationFolder()
            };

            configurationService.SaveConfigurationAsync().Wait();

            Assert.IsTrue(File.Exists(TestConfigurationFilePath), "The config file wasn't saved.");

            configurationService.LoadConfigurationAsync().Wait();

            Assert.IsTrue(configurationService.Configuration.ApplicationFolders.Any(), "Verifying the saved configuration failed.");

            File.Delete(TestConfigurationFilePath);
        }
    }
}
