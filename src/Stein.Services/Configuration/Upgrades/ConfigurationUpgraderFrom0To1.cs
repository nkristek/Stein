using System;
using System.Collections.Generic;
using System.Linq;
using Stein.Common.Configuration;

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
        /// <exception cref="InvalidFileVersionException">When the file version of the given <paramref name="configuration"/> is not supported by this <see cref="IConfigurationUpgrader"/>.</exception>
        public IConfiguration Upgrade(IConfiguration configuration)
        {
            if (configuration.FileVersion != SourceFileVersion)
                throw new InvalidFileVersionException(SourceFileVersion, configuration.FileVersion);

            if (!(configuration is Common.Configuration.v0.Configuration sourceConfiguration))
                throw new Exception("The given configuration ");

            return new Common.Configuration.v1.Configuration
            {
                SelectedTheme = sourceConfiguration.SelectedTheme,
                Applications = sourceConfiguration.ApplicationFolders.Select(af => new Common.Configuration.v1.Application
                {
                    Id = af.Id,
                    Name = af.Name,
                    EnableSilentInstallation = af.EnableSilentInstallation,
                    DisableReboot = af.DisableReboot,
                    EnableInstallationLogging = af.EnableInstallationLogging,
                    AutomaticallyDeleteInstallationLogs = af.AutomaticallyDeleteInstallationLogs,
                    KeepNewestInstallationLogs = af.KeepNewestInstallationLogs,
                    Configuration = new Common.Configuration.v1.InstallerFileBundleProviderConfiguration("Disk", new Dictionary<string, string> { { "Path", af.Path } })
                }).ToList()
            };
        }
    }
}
