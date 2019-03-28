using System.Collections.Generic;

namespace Stein.Services.InstallerFiles.Base
{
    /// <summary>
    /// A configurator to configure a <see cref="IInstallerFileBundleProvider"/>.
    /// </summary>
    public interface IInstallerFileBundleProviderConfigurator
    {
        /// <summary>
        /// Validate the entered information.
        /// </summary>
        /// <returns><c>true</c> if the inputs are valid, <c>false</c> otherwise.</returns>
        bool Validate();

        /// <summary>
        /// Save the configuration to a dictionary.
        /// </summary>
        /// <returns>The dictionary containing the configuration.</returns>
        IDictionary<string, string> SaveConfiguration();

        /// <summary>
        /// Load the configuration data from the given dictionary.
        /// </summary>
        /// <param name="configuration">Dictionary containing the configuration data to load.</param>
        void LoadConfiguration(IDictionary<string, string> configuration);
    }
}
