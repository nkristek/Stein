using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.InstallerFiles.Disk
{
    public class DiskInstallerFileBundleProvider
        : IInstallerFileBundleProvider
    {
        public DiskInstallerFileBundleProvider(IInstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.ProviderType != ProviderType)
                throw new ArgumentException($"Invalid provider type (expected \"{ProviderType}\", got \"{configuration.ProviderType}\"", nameof(configuration));

            if (configuration.Parameters.TryGetValue(nameof(Path), out var path))
                Path = path;
        }

        /// <inheritdoc />
        public string ProviderType => "Disk";

        /// <inheritdoc />
        public string ProviderLink => Path;

        /// <summary>
        /// The path of the folder which contains folders which contain installer files (.msi ending).
        /// </summary>
        public string Path { get; }

        /// <inheritdoc />
        public async Task<IEnumerable<IInstallerFileBundle>> GetInstallerFileBundlesAsync(CancellationToken cancellationToken = default)
        {
            var folder = new DirectoryInfo(Path);
            return await Task.Run(() =>
            {
                return folder.EnumerateDirectories().Select(GetInstallerFileBundle).Where(b => b.InstallerFiles.Any()).OrderBy(b => b.Created).ToList();
            }, cancellationToken);
        }

        private IInstallerFileBundle GetInstallerFileBundle(DirectoryInfo bundleDirectory)
        {
            if (bundleDirectory == null)
                throw new ArgumentNullException(nameof(bundleDirectory));
            
            return new DiskInstallerFileBundle
            {
                Name = bundleDirectory.Name,
                Created = bundleDirectory.CreationTime,
                InstallerFiles = GetInstallerFiles(bundleDirectory).ToList()
            };
        }

        private IEnumerable<IInstallerFile> GetInstallerFiles(DirectoryInfo bundleDirectory)
        {
            if (bundleDirectory == null)
                throw new ArgumentNullException(nameof(bundleDirectory));

            foreach (var file in bundleDirectory.EnumerateFiles("*.msi"))
                yield return new DiskInstallerFile(file.FullName)
                {
                    FileName = file.Name,
                    Created = file.CreationTime
                };
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
