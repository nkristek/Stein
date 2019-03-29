using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private readonly string _configurationFolderPath;

        private readonly string _configurationFileNamePrefix;

        private static readonly IConfigurationUpgradeManager UpgradeManager = new ConfigurationUpgradeManager();

        private static readonly IReadOnlyList<Type> AllConfigurationTypes = GetAllAvailableConfigurationTypes();

        /// <summary>
        /// Get all types that implement <see cref="IConfiguration"/>.
        /// </summary>
        /// <returns>A list all types that implement <see cref="IConfiguration"/>.</returns>
        private static IReadOnlyList<Type> GetAllAvailableConfigurationTypes()
        {
            // TODO
            //return AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(assembly => assembly.GetTypes())
            //    .Where(type => !type.IsAbstract && !type.IsInterface && typeof(IConfiguration).IsAssignableFrom(type))
            //    .ToList();
            return new List<Type>
            {
                typeof(v0.Configuration),
                typeof(v1.Configuration),
                typeof(v2.Configuration)
            };
        }

        public ConfigurationService()
        {
            _configurationFileNamePrefix = "config";
            _configurationFolderPath = GetDefaultConfigurationFolderPath();
        }

        private static string GetDefaultConfigurationFolderPath()
        {
            var appDataConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetEntryAssembly().GetName().Name);
            if (!Directory.Exists(appDataConfigurationPath))
                Directory.CreateDirectory(appDataConfigurationPath);
            return appDataConfigurationPath;
        }

        private string GetConfigurationFilePath(IConfiguration configuration)
        {
            var fileName = String.Concat(_configurationFileNamePrefix, ".v", configuration.FileVersion, ".xml");
            return Path.Combine(_configurationFolderPath, fileName);
        }

        public ConfigurationService(string configurationName, string configurationFolderPath)
        {
            if (String.IsNullOrEmpty(configurationName))
                throw new ArgumentNullException(nameof(configurationName));

            if (String.IsNullOrEmpty(configurationFolderPath))
                throw new ArgumentNullException(nameof(configurationFolderPath));

            _configurationFileNamePrefix = configurationName;
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
            
            if (UpgradeManager.UpgradeToLatestFileVersion(configuration, out var upgradedConfiguration))
                await ToFileAsync(upgradedConfiguration, GetConfigurationFilePath(Configuration));
            
            return upgradedConfiguration as v2.Configuration ?? throw new Exception("The upgrade didn't upgrade the configuration to the latest version.");
        }

        private static IEnumerable<string> GetExistingConfigFileNames(string folderPath, string configFileNamePrefix)
        {
            return Directory.EnumerateFiles(folderPath, $"{configFileNamePrefix}.v*.xml");
        }

        private Tuple<string, Type> CreateLatestConfigurationType(IEnumerable<string> fileNames)
        {
            var fileNamesWithDescendingVersion = fileNames.Select(fileName =>
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
                .ToList();

            Tuple<string, IConfiguration> fileNameWithMatchingConfiguration = null;
            if (fileNamesWithDescendingVersion.Count == 0)
            {
                var oldConfigFileName = Path.Combine(_configurationFolderPath, "Config.xml");
                if (!File.Exists(oldConfigFileName))
                    throw new Exception("No configuration file found.");

                fileNameWithMatchingConfiguration = new Tuple<string, IConfiguration>(oldConfigFileName, new v0.Configuration());
            }
            else
            {
                var configurations = AllConfigurationTypes.Select(Activator.CreateInstance).OfType<IConfiguration>().ToList();
                foreach (var fileNameWithVersion in fileNamesWithDescendingVersion)
                {
                    var matchingConfiguration = configurations.FirstOrDefault(c => c.FileVersion == fileNameWithVersion.Item2);
                    if (matchingConfiguration == null)
                        continue;
                    fileNameWithMatchingConfiguration = new Tuple<string, IConfiguration>(fileNameWithVersion.Item1, matchingConfiguration);
                    break;
                }

                if (fileNameWithMatchingConfiguration == null)
                    throw new Exception("No configuration type found to deserialize the configuration.");
            }

            return new Tuple<string, Type>(fileNameWithMatchingConfiguration.Item1, fileNameWithMatchingConfiguration.Item2.GetType());
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

        /// <inheritdoc />
        public async Task SaveConfigurationAsync()
        {
            await ToFileAsync(Configuration, GetConfigurationFilePath(Configuration));
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
