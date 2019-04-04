using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Services.InstallerFiles.Base
{
    /// <summary>
    /// A provider for installer files.
    /// </summary>
    public interface IInstallerFileBundleProvider
        : IDisposable
    {
        /// <summary>
        /// The type of this provider.
        /// </summary>
        string ProviderType { get; }

        /// <summary>
        /// Link to the destination of the provider. Can be a web url or file system path.
        /// </summary>
        string ProviderLink { get; }

        /// <summary>
        /// Create an async <see cref="Task"/> to get all installer file bundles.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>An async <see cref="Task"/> which gets all installer file bundles.</returns>
        Task<IEnumerable<IInstallerFileBundle>> GetInstallerFileBundlesAsync(CancellationToken cancellationToken = default);
    }
}
