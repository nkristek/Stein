using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Stein.Services.Types;

namespace Stein.Services
{
    public class ConfigurationService
        : IConfigurationService
    {
        public Configuration Configuration { get; private set; } = new Configuration();

        private readonly string _configuationPath;

        public ConfigurationService()
        {
            _configuationPath = GetDefaultConfigurationFilePath();
        }

        private static string GetDefaultConfigurationFilePath()
        {
            var appDataConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetEntryAssembly().GetName().Name);
            if (!Directory.Exists(appDataConfigurationPath))
                Directory.CreateDirectory(appDataConfigurationPath);
            return Path.Combine(appDataConfigurationPath, "Config.xml");
        }

        public ConfigurationService(string configurationFilePath)
        {
            if (String.IsNullOrEmpty(configurationFilePath))
                throw new ArgumentNullException(nameof(configurationFilePath));

            _configuationPath = configurationFilePath;
        }

        public void LoadConfiguration()
        {
            Configuration = Configuration.CreateFromFile(_configuationPath);
        }
        
        public async Task LoadConfigurationAsync()
        {
            Configuration = await Configuration.CreateFromFileAsync(_configuationPath);
        }
        
        public void SaveConfiguration()
        {
            Configuration.ToFile(_configuationPath);
        }
        
        public async Task SaveConfigurationAsync()
        {
            await Configuration.ToFileAsync(_configuationPath);
        }
    }
}
