using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.ViewModels.Types
{
    public interface IInstallerFileProvider
    {
        /// <summary>
        /// Save the installer file to a specified path asynchronously.
        /// </summary>
        /// <param name="filePath">The path where the installer file should be saved to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the asynchronous operation.</param>
        /// <returns>The <see cref="Task"/> of saving the file asynchronously.</returns>
        Task SaveFileAsync(string filePath, IProgress<double>? progress = null, CancellationToken cancellationToken = default);
    }
}
