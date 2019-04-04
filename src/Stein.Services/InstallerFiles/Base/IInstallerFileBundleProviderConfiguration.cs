using System.Collections.Generic;

namespace Stein.Services.InstallerFiles.Base
{
    /// <summary>
    /// A configuration to configure an <see cref="IInstallerFileBundleProvider"/>.
    /// </summary>
    public interface IInstallerFileBundleProviderConfiguration
    {
        /// <summary>
        /// Type of the provider this configuration belongs to.
        /// </summary>
        string ProviderType { get; }

        /// <summary>
        /// Configuration parameters of the provider.
        /// </summary>
        IDictionary<string, string> Parameters { get; }
    }
}
