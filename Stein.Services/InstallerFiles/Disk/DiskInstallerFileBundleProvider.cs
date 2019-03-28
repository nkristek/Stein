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
        /// <inheritdoc />
        public string Type => "Disk";
        
        private readonly DiskInstallerFileBundleProviderConfigurator _configurator = new DiskInstallerFileBundleProviderConfigurator();

        /// <inheritdoc />
        public IInstallerFileBundleProviderConfigurator Configurator => _configurator;

        /// <summary>
        /// The path of the folder which contains folders which contain installer files (.msi ending).
        /// </summary>
        public string Path => _configurator.Path;

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
    }
}
