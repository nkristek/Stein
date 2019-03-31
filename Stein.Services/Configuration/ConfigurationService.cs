using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stein.Services.Configuration
{
    /// <inheritdoc />
    public class ConfigurationService
        : IConfigurationService
    {
        /// <inheritdoc />
        public v2.Configuration Configuration { get; private set; } = new v2.Configuration();

        private readonly IConfigurationFactory _configurationFactory;

        private readonly IConfigurationUpgradeManager _upgradeManager;
        
        private readonly string _configurationFileNamePrefix;

        private readonly string _configurationFolderPath;

        public ConfigurationService(IConfigurationFactory configurationFactory, IConfigurationUpgradeManager upgradeManager, string configurationFileNamePrefix, string configurationFolderPath)
        {
            _configurationFactory = configurationFactory ?? throw new ArgumentNullException(nameof(configurationFactory));
            _upgradeManager = upgradeManager ?? throw new ArgumentNullException(nameof(upgradeManager));

            if (String.IsNullOrEmpty(configurationFileNamePrefix))
                throw new ArgumentNullException(nameof(configurationFileNamePrefix));
            _configurationFileNamePrefix = configurationFileNamePrefix;

            if (String.IsNullOrEmpty(configurationFolderPath))
                throw new ArgumentNullException(nameof(configurationFolderPath));
            _configurationFolderPath = configurationFolderPath;
        }
        
        /// <inheritdoc />
        public async Task LoadConfigurationAsync()
        {
            Configuration = await CreateFromFileAndUpgradeAsync();
        }
        
        private async Task<v2.Configuration> CreateFromFileAndUpgradeAsync()
        {
            var fileNames = GetExistingConfigFileNames(_configurationFolderPath, _configurationFileNamePrefix).ToList();
            var fileNameWithMatchingConfigurationType = CreateLatestConfigurationType(fileNames);
            var configuration = DeserializeConfiguration(fileNameWithMatchingConfigurationType.Item1, fileNameWithMatchingConfigurationType.Item2);
            if (_upgradeManager.UpgradeToLatestFileVersion(configuration, out var upgradedConfiguration))
                await ToFileAsync(upgradedConfiguration);
            return upgradedConfiguration as v2.Configuration ?? throw new Exception("The upgrade didn't upgrade the configuration to the latest version.");
        }

        private static IEnumerable<string> GetExistingConfigFileNames(string folderPath, string configFileNamePrefix)
        {
            return Directory.EnumerateFiles(folderPath, $"{configFileNamePrefix}.v*.xml");
        }

        private Tuple<string, Type> CreateLatestConfigurationType(IEnumerable<string> fileNames)
        {
            var fileNameWithHighestVersion = fileNames.Select(fileName =>
                {
                    var splitFileName = fileName.Split('.');
                    if (splitFileName.Length != 3)
                        return new Tuple<string, long?>(fileName, null);
                    var fileVersionString = String.Join(String.Empty, splitFileName[1].Skip(1));
                    return long.TryParse(fileVersionString, out var fileVersion)
                        ? new Tuple<string, long?>(fileName, fileVersion)
                        : new Tuple<string, long?>(fileName, null);
                })
                .Where(f => f.Item2.HasValue)
                .Select(f => new Tuple<string, long>(f.Item1, f.Item2.Value))
                .OrderByDescending(f => f.Item2)
                .FirstOrDefault();
            
            if (fileNameWithHighestVersion == null)
            {
                var oldConfigFileName = Path.Combine(_configurationFolderPath, "Config.xml");
                if (!File.Exists(oldConfigFileName))
                    throw new Exception("No configuration file found.");

                return new Tuple<string, Type>(oldConfigFileName, typeof(v0.Configuration));
            }

            var configuration = _configurationFactory.Create(fileNameWithHighestVersion.Item2);
            return new Tuple<string, Type>(fileNameWithHighestVersion.Item1, configuration.GetType());
        }

        private static IConfiguration DeserializeConfiguration(string configurationFileName, Type configurationType)
        {
            if (String.IsNullOrEmpty(configurationFileName))
                throw new ArgumentNullException(nameof(configurationFileName));
            if (configurationType == null)
                throw new ArgumentNullException(nameof(configurationType));
            
            var xmlSerializer = new XmlSerializer(configurationType, configurationType.GetNestedTypes());
            using (var fileReader = new StreamReader(configurationFileName))
                return xmlSerializer.Deserialize(fileReader) as IConfiguration;
        }

        private string GetConfigurationFilePath(IConfiguration configuration)
        {
            if (!Directory.Exists(_configurationFolderPath))
                Directory.CreateDirectory(_configurationFolderPath);
            var fileName = String.Concat(_configurationFileNamePrefix, ".v", configuration.FileVersion, ".xml");
            return Path.Combine(_configurationFolderPath, fileName);
        }

        /// <inheritdoc />
        public async Task SaveConfigurationAsync()
        {
            await ToFileAsync(Configuration);
        }

        private async Task ToFileAsync(IConfiguration configuration)
        {
            await ToFileAsync(configuration, GetConfigurationFilePath(configuration));
        }

        private static async Task ToFileAsync(IConfiguration configuration, string filePath)
        {
            await Task.Run(() => ToFile(configuration, filePath));
        }

        private static void ToFile(IConfiguration configuration, string filePath)
        {
            var configurationType = configuration.GetType();
            var xmlSerializer = new XmlSerializer(configurationType, configurationType.GetNestedTypes());
            using (var writer = new StreamWriter(filePath))
                xmlSerializer.Serialize(writer, configuration);
        }
    }
}
