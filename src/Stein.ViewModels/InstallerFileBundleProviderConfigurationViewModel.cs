using NKristek.Smaragd.ViewModels;
using Stein.Common.Configuration.v2;

namespace Stein.ViewModels
{
    public abstract class InstallerFileBundleProviderViewModel
        : ViewModel
    {
        /// <summary>
        /// Type of the provider.
        /// </summary>
        public abstract string ProviderType { get; }

        /// <summary>
        /// Load the given configuration.
        /// </summary>
        /// <param name="configuration">Configuration to load.</param>
        public abstract void LoadConfiguration(InstallerFileBundleProviderConfiguration configuration);

        /// <summary>
        /// Create the configuration from the data set in the <see cref="InstallerFileBundleProviderViewModel"/>.
        /// </summary>
        /// <returns>The configuration which includes the data from this <see cref="InstallerFileBundleProviderViewModel"/>.</returns>
        public abstract InstallerFileBundleProviderConfiguration CreateConfiguration();
    }
}
