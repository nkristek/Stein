using System;
using System.Threading.Tasks;
using Stein.Types.ConfigurationTypes;

namespace Stein.Services
{
    public class ConfigurationService
        : IConfigurationService
    {
        public Configuration Configuration { get; private set; } = new Configuration();

        private readonly string _configuationPath;

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
