using System;
using System.Linq;

namespace Stein.Services.Configuration.Upgrades
{
    public class ConfigurationUpgraderFrom1To2
        : IConfigurationUpgrader
    {
        /// <inheritdoc />
        public long SourceFileVersion => 1;

        /// <inheritdoc />
        public long TargetFileVersion => 2;

        /// <inheritdoc />
        public IConfiguration Upgrade(IConfiguration configuration)
        {
            if (configuration.FileVersion != SourceFileVersion)
                throw new InvalidFileVersionException(SourceFileVersion, configuration.FileVersion);

            if (!(configuration is v1.Configuration sourceConfiguration))
                throw new Exception("The given configuration ");

            return new v2.Configuration
            {
                SelectedTheme = sourceConfiguration.SelectedTheme,
                Applications = sourceConfiguration.Applications.Select(a => new v2.Application
                {
                    Id = a.Id,
                    Name = a.Name,
                    EnableSilentInstallation = a.EnableSilentInstallation,
                    DisableReboot = a.DisableReboot,
                    EnableInstallationLogging = a.EnableInstallationLogging,
                    AutomaticallyDeleteInstallationLogs = a.AutomaticallyDeleteInstallationLogs,
                    KeepNewestInstallationLogs = a.KeepNewestInstallationLogs,
                    FilterDuplicateInstallers = true,
                    Configuration = new v2.InstallerFileBundleProviderConfiguration(a.Configuration.Type, a.Configuration.ToDictionary())
                }).ToList()
            };
        }
    }
}
