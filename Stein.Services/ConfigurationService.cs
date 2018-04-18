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

        private readonly string _configuationPath;

        public ConfigurationService(string configurationFilePath)
        {
            if (String.IsNullOrEmpty(configurationFilePath))
                throw new ArgumentNullException(nameof(configurationFilePath));

            _configuationPath = configurationFilePath;
        }

        public void LoadConfigurationFromDisk()
        {
            Configuration = Configuration.CreateFromFile(_configuationPath);
        }
        
        public async Task LoadConfigurationFromDiskAsync()
        {
            Configuration = await Configuration.CreateFromFileAsync(_configuationPath);
        }
        
        public void SaveConfigurationToDisk()
        {
            Configuration.ToFile(_configuationPath);
        }
        
        public async Task SaveConfigurationToDiskAsync()
        {
            await Configuration.ToFileAsync(_configuationPath);
        }
    }
}
