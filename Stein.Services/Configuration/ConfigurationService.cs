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

        private readonly string _configurationName;

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
            _configurationName = "config";
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
            var fileName = String.Concat(_configurationName, ".v", configuration.FileVersion, ".xml");
            return Path.Combine(_configurationFolderPath, fileName);
        }

        public ConfigurationService(string configurationName, string configurationFolderPath)
        {
            if (String.IsNullOrEmpty(configurationName))
                throw new ArgumentNullException(nameof(configurationName));

            if (String.IsNullOrEmpty(configurationFolderPath))
                throw new ArgumentNullException(nameof(configurationFolderPath));

            _configurationName = configurationName;
            _configurationFolderPath = configurationFolderPath;
        }

        /// <inheritdoc />
        public async Task LoadConfigurationAsync()
        {
            Configuration = await CreateFromFileAndUpgradeAsync();
        }

        // TODO: refactor to multiple methods
        private async Task<v2.Configuration> CreateFromFileAndUpgradeAsync()
        {
            // get all file names in the folder according to the naming convention and parse the file version encoded in the file name

            var fileNames = Directory.EnumerateFiles(_configurationFolderPath, $"{_configurationName}.v*.xml")
                .Select(fileName =>
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

            // get the file name with the highest file version which has a matching configuration instance

            Tuple<string, IConfiguration> fileNameWithMatchingConfiguration = null;
            if (fileNames.Count == 0)
            {
                var oldConfigFileName = Path.Combine(_configurationFolderPath, "Config.xml");
                if (!File.Exists(oldConfigFileName))
                    return new v2.Configuration();

                fileNameWithMatchingConfiguration = new Tuple<string, IConfiguration>(oldConfigFileName, new v0.Configuration());
            }
            else
            {
                foreach (var fileName in fileNames)
                {
                    var matchingConfiguration = AllConfigurationTypes
                        .Select(Activator.CreateInstance)
                        .OfType<IConfiguration>()
                        .FirstOrDefault(c => c.FileVersion == fileName.Item2);
                    if (matchingConfiguration == null)
                        continue;
                    fileNameWithMatchingConfiguration = new Tuple<string, IConfiguration>(fileName.Item1, matchingConfiguration);
                    break;
                }
                
                if (fileNameWithMatchingConfiguration == null)
                    throw new Exception("No configuration type found to deserialize the configuration.");
            }
            
            // deserialize the configuration file using the matching configuration type

            var foundConfigurationType = fileNameWithMatchingConfiguration.Item2.GetType();
            var xmlSerializer = new XmlSerializer(foundConfigurationType, foundConfigurationType.GetNestedTypes());
            IConfiguration deserializedConfiguration;
            using (var fileReader = new StreamReader(fileNameWithMatchingConfiguration.Item1))
                deserializedConfiguration = xmlSerializer.Deserialize(fileReader) as IConfiguration;

            // upgrade the configuration to the latest version

            if (UpgradeManager.UpgradeToLatestFileVersion(deserializedConfiguration, out var upgradedConfiguration))
                await ToFileAsync(upgradedConfiguration, GetConfigurationFilePath(Configuration));
            
            return upgradedConfiguration as v2.Configuration ?? throw new Exception("The upgrade didn't upgrade the configuration to the latest version.");
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
