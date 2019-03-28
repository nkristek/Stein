using System;
using System.Collections.Generic;
using System.Linq;
using Stein.Services.Configuration.Upgrades;

namespace Stein.Services.Configuration
{
    public class ConfigurationUpgradeManager
        : IConfigurationUpgradeManager
    {
        private static readonly IReadOnlyList<IConfigurationUpgrader> AllConfigurationUpgraders = GetAllAvailableConfigurationUpgraders();

        /// <summary>
        /// Create instances of all types that implement <see cref="IConfigurationUpgrader"/>.
        /// </summary>
        /// <returns>A list of instances of all types that implement <see cref="IConfigurationUpgrader"/>.</returns>
        private static IReadOnlyList<IConfigurationUpgrader> GetAllAvailableConfigurationUpgraders()
        {
            // TODO
            //return AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(assembly => assembly.GetTypes())
            //    .Where(type => !type.IsAbstract && !type.IsInterface && typeof(IConfigurationUpgrader).IsAssignableFrom(type))
            //    .Select(Activator.CreateInstance).OfType<IConfigurationUpgrader>()
            //    .OrderBy(u => u.SourceFileVersion)
            //    .ToList();
            return new List<IConfigurationUpgrader>
            {
                new ConfigurationUpgraderFrom0To1(),
            };
        }

        /// <inheritdoc />
        public bool UpgradeToLatestFileVersion(IConfiguration configuration, out IConfiguration upgradedConfiguration)
        {
            // TODO: currently an upgrader should only upgrade in steps of one, maybe use something like dijkstra to find the most minimal count of upgrades instead
            
            var currentConfiguration = configuration;
            foreach (var upgrader in AllConfigurationUpgraders)
            {
                if (currentConfiguration.FileVersion < upgrader.SourceFileVersion)
                    throw new Exception($"No upgrade was found to upgrade the configuration from {currentConfiguration.FileVersion} to {AllConfigurationUpgraders.Max(u => u.TargetFileVersion)}.");
                if (currentConfiguration.FileVersion == upgrader.SourceFileVersion)
                    currentConfiguration = upgrader.Upgrade(currentConfiguration);
            }

            upgradedConfiguration = currentConfiguration;
            return upgradedConfiguration.FileVersion != configuration.FileVersion;
        }
    }
}
