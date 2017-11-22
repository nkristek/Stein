using System;
using System.IO;
using System.Threading.Tasks;
using Stein.ConfigurationTypes;

namespace Stein.Services
{
    public static class ConfigurationService
    {
        public static Configuration Configuration { get; private set; }

        private static string _ConfigurationFolderPath;
        public static string ConfiguationFolderPath
        {
            get
            {
                return _ConfigurationFolderPath;
            }

            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                _ConfigurationFolderPath = value;
            }
        }

        public static string ConfiguationPath
        {
            get
            {
                if (String.IsNullOrEmpty(ConfiguationFolderPath))
                    throw new Exception("ConfiguationFolderPath not set.");

                return Path.Combine(ConfiguationFolderPath, "Config.xml");
            }
        }
        
        public static void LoadConfigurationFromDisk()
        {
            try
            {
                Configuration = Configuration.CreateFromFile(ConfiguationPath);
            }
            catch
            {
                Configuration = new Configuration();
            }
        }

        public static async Task LoadConfigurationFromDiskAsync()
        {
            try
            {
                Configuration = await Configuration.CreateFromFileAsync(ConfiguationPath);
            }
            catch
            {
                Configuration = new Configuration();
            }
        }
        
        public static void SaveConfigurationToDisk()
        {
            try
            {
                Configuration?.ToFile(ConfiguationPath);
            }
            catch (Exception exception)
            {
                throw new Exception("Saving the configuration file failed.", exception);
            }
        }

        public static async Task SaveConfigurationToDiskAsync()
        {
            try
            {
                await Configuration?.ToFileAsync(ConfiguationPath);
            }
            catch (Exception exception)
            {
                throw new Exception("Saving the configuration file failed.", exception);
            }
        }
    }
}
