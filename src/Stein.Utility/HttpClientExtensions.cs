using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Utility
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
        public static Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (String.IsNullOrEmpty(requestUri))
                throw new ArgumentNullException(nameof(requestUri));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            IProgress<DownloadProgress> progressReporter = null;
            if (progress != null)
                progressReporter = new Progress<DownloadProgress>(p => progress.Report(p.BytesTotal > 0L ? ((double) p.BytesDownloaded / p.BytesTotal) : 0L));
            return client.DownloadAsync(requestUri, destination, progressReporter, cancellationToken);
        }

        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destination">The destination to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        public static Task DownloadAsync(this HttpClient client, Uri requestUri, Stream destination, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            IProgress<DownloadProgress> progressReporter = null;
            if (progress != null)
                progressReporter = new Progress<DownloadProgress>(p => progress.Report(p.BytesTotal > 0L ? ((double)p.BytesDownloaded / p.BytesTotal) : 0L));
            return client.DownloadAsync(requestUri, destination, progressReporter, cancellationToken);
        }

        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destination">The destination to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<DownloadProgress> progress = null, CancellationToken cancellationToken = default)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            
            if (String.IsNullOrEmpty(requestUri))
                throw new ArgumentNullException(nameof(requestUri));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                response.EnsureSuccessStatusCode();
                await response.DownloadAsync(destination, progress, cancellationToken);
            }
        }

        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destination">The destination to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, Stream destination, IProgress<DownloadProgress> progress = null, CancellationToken cancellationToken = default)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                response.EnsureSuccessStatusCode();
                await response.DownloadAsync(destination, progress, cancellationToken);
            }
        }

        private static async Task DownloadAsync(this HttpResponseMessage response, Stream destination, IProgress<DownloadProgress> progress = null, CancellationToken cancellationToken = default)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            using (var download = await response.Content.ReadAsStreamAsync())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (progress == null)
                {
                    await download.CopyToAsync(destination, 81920, cancellationToken);
                    return;
                }

                var bytesTotal = response.Content.Headers.ContentLength ?? 0;
                var progressReporter = new Progress<long>(bytesDownloaded => progress.Report(new DownloadProgress
                {
                    BytesDownloaded = bytesDownloaded,
                    BytesTotal = bytesTotal
                }));
                await download.CopyToAsync(destination, 81920, progressReporter, cancellationToken);
            }
        }
    }
}
