using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TempManager.ViewModels;
using WpfBase.ViewModels;

namespace TempManager.Configuration
{
    public static class AppConfigurationService
    {
        private static AppConfiguration _CurrentConfiguration;

        public static AppConfiguration CurrentConfiguration
        {
            get
            {
                if (_CurrentConfiguration == null)
                {
                    _CurrentConfiguration = ReadAppConfiguration();

                    SaveConfiguration();
                }
                return _CurrentConfiguration;
            }

            set
            {
                _CurrentConfiguration = value;
            }
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
                messageBuilder.AppendLine("Reading the configuration file failed. A new file will now be created.");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(exception.Message);
                MessageBox.Show(messageBuilder.ToString());

                return new AppConfiguration();
            }
        }

        private static string AppConfiguationPath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appConfigurationDirectoryName = "TempManager";
                var appConfigurationDirectoryPath = Path.Combine(appDataPath, appConfigurationDirectoryName);
                if (!Directory.Exists(appConfigurationDirectoryPath))
                    Directory.CreateDirectory(appConfigurationDirectoryPath);

                var appConfigurationFileName = "Config.xml";
                return Path.Combine(appConfigurationDirectoryPath, appConfigurationFileName);
            }
        }

        public static bool SaveConfiguration()
        {
            try
            {
                _CurrentConfiguration.ToXmlFile(AppConfiguationPath);
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
