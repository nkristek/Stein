using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Stein.Common.MsiService;

namespace Stein.Common.InstallerFiles
{
    /// <summary>
    /// An installer file.
    /// </summary>
    public interface IInstallerFile
    {
        /// <summary>
        /// Name of the installer file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Name of the installer (read from the properties of the MSI file). Maybe <c>null</c> if the file isn't downloaded yet.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The date the installer was created (read from the properties of the MSI file). Maybe <c>null</c> if the file isn't downloaded yet.
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// The culture of the installer (read from the properties of the MSI file). Maybe <c>null</c> if the file isn't downloaded yet.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// The version of the installer (read from the properties of the MSI file). Maybe <c>null</c> if the file isn't downloaded yet.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// ProductCode of the installer (read from the properties of the MSI file). Maybe <c>null</c> if the file isn't downloaded yet.
        /// </summary>
        string ProductCode { get; }

        /// <summary>
        /// Save the installer file to a specified path asynchronously.
        /// </summary>
        /// <param name="filePath">The path where the installer file should be saved to.</param>
        /// <param name="msiService">The MsiService to read the metadata from the installer file.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the asynchronous operation.</param>
        /// <returns>The <see cref="Task"/> of saving the file asynchronously.</returns>
        Task SaveFileAsync(string filePath, IMsiService msiService, IProgress<double> progress = null, CancellationToken cancellationToken = default);
    }
}
