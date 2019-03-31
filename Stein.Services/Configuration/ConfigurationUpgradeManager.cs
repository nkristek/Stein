using System;
using System.Linq;
using Stein.Services.Configuration.Upgrades;

namespace Stein.Services.Configuration
{
    public class ConfigurationUpgradeManager
        : IConfigurationUpgradeManager
    {
        private readonly IConfigurationUpgraderFactory _upgraderFactory;

        public ConfigurationUpgradeManager(IConfigurationUpgraderFactory upgraderFactory)
        {
            _upgraderFactory = upgraderFactory ?? throw new ArgumentNullException(nameof(upgraderFactory));
        }

        /// <inheritdoc />
        public bool UpgradeToLatestFileVersion(IConfiguration configuration, out IConfiguration upgradedConfiguration)
        {
            var currentConfiguration = configuration;
            foreach (var upgrader in _upgraderFactory.CreateAll().OrderBy(u => u.SourceFileVersion))
            {
                if (currentConfiguration.FileVersion < upgrader.SourceFileVersion)
                    throw new Exception("No upgrade was found to upgrade the configuration.");
                if (currentConfiguration.FileVersion == upgrader.SourceFileVersion)
                    currentConfiguration = upgrader.Upgrade(currentConfiguration);
            }

            upgradedConfiguration = currentConfiguration;
            return upgradedConfiguration.FileVersion != configuration.FileVersion;
        }
    }
}
