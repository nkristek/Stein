using System;
using System.IO;
using System.Threading.Tasks;
using Stein.Types.ConfigurationTypes;

namespace Stein.Services
{
    public class ConfigurationService
        : IConfigurationService
    {
        public static IConfigurationService Instance;

        public Configuration Configuration { get; private set; }

        private string _ConfiguationPath;

        public ConfigurationService(string configurationFilePath)
        {
            if (String.IsNullOrEmpty(configurationFilePath))
                throw new ArgumentNullException("configurationFolderPath");

            _ConfiguationPath = configurationFilePath;
        }

        public void LoadConfigurationFromDisk()
        {
            Configuration = Configuration.CreateFromFile(_ConfiguationPath);
        }
        
        public async Task LoadConfigurationFromDiskAsync()
        {
            Configuration = await Configuration.CreateFromFileAsync(_ConfiguationPath);
        }
        
        public void SaveConfigurationToDisk()
        {
            Configuration.ToFile(_ConfiguationPath);
        }
        
        public async Task SaveConfigurationToDiskAsync()
        {
            await Configuration.ToFileAsync(_ConfiguationPath);
        }
    }
}
