using System;
using System.IO;
using System.Threading.Tasks;
using Stein.ConfigurationTypes;

namespace Stein.Services
{
    public static class ConfigurationService
    {
        /// <summary>
        /// Configuration which gets loaded from and saved to the file system
        /// </summary>
        public static Configuration Configuration { get; private set; }

        private static string _ConfigurationFolderPath;
        /// <summary>
        /// Path to the folder in which the configuration file exists
        /// </summary>
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

        /// <summary>
        /// Path to the configuration file
        /// </summary>
        public static string ConfiguationPath
        {
            get
            {
                if (String.IsNullOrEmpty(ConfiguationFolderPath))
                    throw new Exception("ConfiguationFolderPath not set.");

                return Path.Combine(ConfiguationFolderPath, "Config.xml");
            }
        }
        
        /// <summary>
        /// Loads the configuration file from the file system
        /// </summary>
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

        /// <summary>
        /// Loads the configuration file from the file system asynchronously
        /// </summary>
        /// <returns>Task which loads the configuration file from the file system</returns>
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
        
        /// <summary>
        /// Saves the configuration file to the file system
        /// </summary>
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

        /// <summary>
        /// Saves the configuration file to the file system asynchronously
        /// </summary>
        /// <returns>>Task which saves the configuration file to the file system</returns>
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
