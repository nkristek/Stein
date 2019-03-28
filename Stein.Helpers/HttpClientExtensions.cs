using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Helpers
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destination">The destination to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                response.EnsureSuccessStatusCode();

                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination, 81920, cancellationToken);
                        progress?.Report(1);
                        return;
                    }

                    var totalBytes = contentLength.Value;
                    var progressReporter = new Progress<long>(bytesDownloaded => progress.Report((double)bytesDownloaded / totalBytes));
                    await download.CopyToAsync(destination, 81920, progressReporter, cancellationToken);
                    progress.Report(1);
                }
            }
        }
    }
}
