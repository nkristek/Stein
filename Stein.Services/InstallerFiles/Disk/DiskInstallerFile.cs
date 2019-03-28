using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Stein.Helpers;
using Stein.Services.InstallerFiles.Base;
using Stein.Services.MsiService;

namespace Stein.Services.InstallerFiles.Disk
{
    /// <summary>
    /// Installer file from the file system.
    /// </summary>
    public class DiskInstallerFile
        : InstallerFile
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Stein.Services.InstallerFiles.Disk.DiskInstallerFile" /> class with the Path of the installer file.
        /// </summary>
        /// <param name="filePath">Path to the installer file on disk.</param>
        public DiskInstallerFile(string filePath) 
        {
            FilePath = filePath;
        }

        /// <summary>
        /// The path to the installer file.
        /// </summary>
        public string FilePath { get; }

        /// <inheritdoc />
        public override async Task SaveFileAsync(string filePath, IMsiService msiService, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(FilePath))
                throw new Exception("FilePath is null");
            
            if (!File.Exists(FilePath))
                throw new Exception("Source file doesn't exist");

            if (File.Exists(filePath))
                throw new ArgumentException("Target file already exists");

            try
            {
                using (var sourceFile = File.OpenRead(FilePath))
                using (var targetFile = File.Create(filePath))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (progress == null)
                    {
                        await sourceFile.CopyToAsync(targetFile, 81920, cancellationToken);
                        return;
                    }

                    var totalBytes = sourceFile.Length;
                    var progressReporter = new Progress<long>(bytesCopied => progress.Report((double)bytesCopied / totalBytes));
                    await sourceFile.CopyToAsync(targetFile, 81920, progressReporter, cancellationToken);
                }

                await ReadMsiMetadata(filePath, msiService);
            }
            catch
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                }

                throw;
            }
        }
    }
}
