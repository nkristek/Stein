using System;
using System.Linq;
using Stein.Common.Configuration;

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
        /// <exception cref="InvalidFileVersionException">When the file version of the given <paramref name="configuration"/> is not supported by this <see cref="IConfigurationUpgrader"/>.</exception>
        public IConfiguration Upgrade(IConfiguration configuration)
        {
            if (configuration.FileVersion != SourceFileVersion)
                throw new InvalidFileVersionException(SourceFileVersion, configuration.FileVersion);

            if (!(configuration is Common.Configuration.v1.Configuration sourceConfiguration))
                throw new Exception("The given configuration ");

            return new Common.Configuration.v2.Configuration
            {
                SelectedTheme = sourceConfiguration.SelectedTheme,
                Applications = sourceConfiguration.Applications.Select(a => new Common.Configuration.v2.Application
                {
                    Id = a.Id,
                    Name = a.Name,
                    EnableSilentInstallation = a.EnableSilentInstallation,
                    DisableReboot = a.DisableReboot,
                    EnableInstallationLogging = a.EnableInstallationLogging,
                    AutomaticallyDeleteInstallationLogs = a.AutomaticallyDeleteInstallationLogs,
                    KeepNewestInstallationLogs = a.KeepNewestInstallationLogs,
                    FilterDuplicateInstallers = true,
                    Configuration = new Common.Configuration.v2.InstallerFileBundleProviderConfiguration(a.Configuration.ProviderType, a.Configuration.Parameters)
                }).ToList()
            };
        }
    }
}
