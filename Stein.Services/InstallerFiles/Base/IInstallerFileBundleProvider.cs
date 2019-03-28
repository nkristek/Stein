using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Services.InstallerFiles.Base
{
    /// <summary>
    /// A provider for installer files. It can be configured using its <see cref="Configurator"/>.
    /// </summary>
    public interface IInstallerFileBundleProvider
    {
        /// <summary>
        /// The type of this provider.
        /// </summary>
        string Type { get; }
        
        /// <summary>
        /// A configurator to configure this <see cref="IInstallerFileBundleProvider"/>.
        /// </summary>
        IInstallerFileBundleProviderConfigurator Configurator { get; }

        /// <summary>
        /// Create an async <see cref="Task"/> to get all installer file bundles.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>An async <see cref="Task"/> which gets all installer file bundles.</returns>
        Task<IEnumerable<IInstallerFileBundle>> GetInstallerFileBundlesAsync(CancellationToken cancellationToken = default);
    }
}
