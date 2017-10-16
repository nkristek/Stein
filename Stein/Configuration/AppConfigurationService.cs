using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Stein.ViewModels;
using WpfBase.ViewModels;

namespace Stein.Configuration
{
    public static class AppConfigurationService
    {
        private static AppConfiguration _CurrentConfiguration;

        public static AppConfiguration CurrentConfiguration
        {
            get
            {
                if (_CurrentConfiguration == null)
                    _CurrentConfiguration = ReadAppConfiguration();
                return _CurrentConfiguration;
            }

            set
            {
                _CurrentConfiguration = value;
                SaveConfiguration();
            }
        }

        public static void ReloadAppConfiguration()
        {
            CurrentConfiguration = ReadAppConfiguration();
        }

        private static AppConfiguration ReadAppConfiguration()
        {
            try
            {
                return AppConfiguration.CreateFromXmlFile(AppConfiguationPath);
            }
            catch (Exception exception)
            {
                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("Reading the configuration file failed. A new one will now be created.");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(exception.Message);
                MessageBox.Show(messageBuilder.ToString());

                var newConfiguration = new AppConfiguration();
                SaveConfiguration(newConfiguration);
                return newConfiguration;
            }
        }

        public static string AppConfigurationFolderPath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appConfigurationDirectoryName = "Stein";
                var appConfigurationDirectoryPath = Path.Combine(appDataPath, appConfigurationDirectoryName);
                if (!Directory.Exists(appConfigurationDirectoryPath))
                    Directory.CreateDirectory(appConfigurationDirectoryPath);

                return appConfigurationDirectoryPath;
            }
        }

        public static string AppConfiguationPath
        {
            get
            {
                var appConfigurationFileName = "Config.xml";
                return Path.Combine(AppConfigurationFolderPath, appConfigurationFileName);
            }
        }

        public static bool SaveConfiguration()
        {
            return SaveConfiguration(CurrentConfiguration);
        }

        private static bool SaveConfiguration(AppConfiguration configuration)
        {
            try
            {
                configuration?.ToXmlFile(AppConfiguationPath);
            }
            catch (Exception createException)
            {
                var errorMessageBuilder = new StringBuilder();
                errorMessageBuilder.AppendLine("Saving the configuration file failed.");
                errorMessageBuilder.AppendLine();
                errorMessageBuilder.AppendLine(createException.Message);
                MessageBox.Show(errorMessageBuilder.ToString());
                return false;
            }
            return true;
        }
    }
}
