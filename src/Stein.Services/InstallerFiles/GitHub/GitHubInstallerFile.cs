using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Stein.Helpers;
using Stein.Services.InstallerFiles.Base;
using Stein.Services.MsiService;

namespace Stein.Services.InstallerFiles.GitHub
{
    /// <summary>
    /// Installer file from a GitHub release.
    /// </summary>
    public class GitHubInstallerFile
        : InstallerFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubInstallerFile" /> class with the Url the installer file can be downloaded from.
        /// </summary>
        /// <param name="downloadUrl">Url to download the file.</param>
        public GitHubInstallerFile(string downloadUrl) 
        {
            DownloadUrl = downloadUrl;
        }

        /// <summary>
        /// The url to download the installer file from.
        /// </summary>
        public string DownloadUrl { get; }

        /// <inheritdoc />
        public override async Task SaveFileAsync(string filePath, IMsiService msiService, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(DownloadUrl))
                throw new Exception("DownloadUrl is null");
            
            if (File.Exists(filePath))
                throw new ArgumentException("File already exists");

            try
            {
                using (var file = File.Create(filePath))
                using (var client = new HttpClient())
                    await client.DownloadAsync(DownloadUrl, file, progress, cancellationToken);
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
