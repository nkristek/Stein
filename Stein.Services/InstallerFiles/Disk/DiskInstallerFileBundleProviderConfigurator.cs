using System;
using System.Collections.Generic;
using System.IO;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.InstallerFiles.Disk
{
    public class DiskInstallerFileBundleProviderConfigurator
        : IInstallerFileBundleProviderConfigurator
    {
        public string Path { get; private set; }
        
        /// <inheritdoc />
        public bool Validate()
        {
            return !String.IsNullOrWhiteSpace(Path) && Directory.Exists(Path);
        }

        /// <inheritdoc />
        public IDictionary<string, string> SaveConfiguration()
        {
            return new Dictionary<string, string> { { nameof(Path), Path } };
        }

        /// <inheritdoc />
        public void LoadConfiguration(IDictionary<string, string> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            
            if (configuration.TryGetValue(nameof(Path), out var path))
                Path = path;
            else
                throw new ArgumentException("The given dictionary does not contain the folder path value.");
        }
    }
}
