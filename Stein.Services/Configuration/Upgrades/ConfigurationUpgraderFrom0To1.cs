using System;
using System.Collections.Generic;
using System.Linq;

namespace Stein.Services.Configuration.Upgrades
{
    public class ConfigurationUpgraderFrom0To1
        : IConfigurationUpgrader
    {
        /// <inheritdoc />
        public long SourceFileVersion => 0;

        /// <inheritdoc />
        public long TargetFileVersion => 1;

        /// <inheritdoc />
        public IConfiguration Upgrade(IConfiguration configuration)
        {
            if (configuration.FileVersion != SourceFileVersion)
                throw new InvalidFileVersionException(SourceFileVersion, configuration.FileVersion);

            if (!(configuration is v0.Configuration sourceConfiguration))
                throw new Exception("The given configuration ");

            return new v1.Configuration
            {
                SelectedTheme = sourceConfiguration.SelectedTheme,
                Applications = sourceConfiguration.ApplicationFolders.Select(af => new v1.Application
                {
                    Id = af.Id,
                    Name = af.Name,
                    EnableSilentInstallation = af.EnableSilentInstallation,
                    DisableReboot = af.DisableReboot,
                    EnableInstallationLogging = af.EnableInstallationLogging,
                    AutomaticallyDeleteInstallationLogs = af.AutomaticallyDeleteInstallationLogs,
                    KeepNewestInstallationLogs = af.KeepNewestInstallationLogs,
                    Configuration = new v1.InstallerFileBundleProviderConfiguration("Disk", new Dictionary<string, string> { { "Path", af.Path } })
                }).ToList()
            };
        }
    }
}
