using System;
using Stein.Common.InstallerFiles;
using Stein.Services.InstallerFiles.Disk;
using Stein.Services.InstallerFiles.GitHub;

namespace Stein.Services.InstallerFiles.Base
{
    public class InstallerFileBundleProviderFactory
        : IInstallerFileBundleProviderFactory
    {
        /// <inheritdoc />
        public IInstallerFileBundleProvider Create(IInstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            switch (configuration.ProviderType)
            {
                case "Disk": return new DiskInstallerFileBundleProvider(configuration);
                case "GitHub": return new GitHubInstallerFileBundleProvider(configuration);
                default: throw new NotSupportedException($"Provider type {configuration.ProviderType} is not supported.");
            }
        }
    }
}
