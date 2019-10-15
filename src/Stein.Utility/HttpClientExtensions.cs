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
        /// <param name="destinationFilePath">The file path to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destinationFilePath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, string requestUri, string destinationFilePath, IProgress<double>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (String.IsNullOrEmpty(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            if (String.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            using var file = File.Create(destinationFilePath);
            await client.DownloadAsync(requestUri, file, progress, cancellationToken, bufferSize);
        }

        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destinationFilePath">The file path to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destinationFilePath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, string requestUri, string destinationFilePath, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (String.IsNullOrEmpty(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            if (String.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            using var file = File.Create(destinationFilePath);
            await client.DownloadAsync(requestUri, file, progress, cancellationToken, bufferSize);
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
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destination"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<double>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (String.IsNullOrEmpty(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            IProgress<DownloadProgress>? progressReporter = null;
            if (progress != null)
                progressReporter = new Progress<DownloadProgress>(p => progress.Report(p.BytesTotal > 0L ? ((double) p.BytesDownloaded / p.BytesTotal) : 0L));

            await client.DownloadAsync(requestUri, destination, progressReporter, cancellationToken, bufferSize);
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
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destination"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (String.IsNullOrEmpty(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            await response.DownloadAsync(destination, progress, cancellationToken, bufferSize);
        }

        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destinationFilePath">The file path to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destinationFilePath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, string destinationFilePath, IProgress<double>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));
            if (String.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            using var file = File.Create(destinationFilePath);
            await client.DownloadAsync(requestUri, file, progress, cancellationToken, bufferSize);
        }

        /// <summary>
        /// Download a file asynchronously providing progress tracking and cancellation.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to download the file.</param>
        /// <param name="requestUri">The URI of the file to download.</param>
        /// <param name="destinationFilePath">The file path to download to.</param>
        /// <param name="progress">An optional progress tracker.</param>
        /// <param name="cancellationToken">A cancellation token to stop the download.</param>
        /// <returns>The <see cref="Task"/> which downloads the file asynchronously.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destinationFilePath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, string destinationFilePath, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));
            if (String.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            using var file = File.Create(destinationFilePath);
            await client.DownloadAsync(requestUri, file, progress, cancellationToken, bufferSize);
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
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destination"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, Stream destination, IProgress<double>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            IProgress<DownloadProgress>? progressReporter = null;
            if (progress != null)
                progressReporter = new Progress<DownloadProgress>(p => progress.Report(p.BytesTotal > 0L ? ((double)p.BytesDownloaded / p.BytesTotal) : 0L));

            await client.DownloadAsync(requestUri, destination, progressReporter, cancellationToken, bufferSize);
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
        /// <exception cref="ArgumentNullException"><paramref name="client"/>, <paramref name="requestUri"/> or <paramref name="destination"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, Stream destination, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            await response.DownloadAsync(destination, progress, cancellationToken, bufferSize);
        }

        private static async Task DownloadAsync(this HttpResponseMessage response, Stream destination, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default, int bufferSize = 81920)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var download = await response.Content.ReadAsStreamAsync();
            cancellationToken.ThrowIfCancellationRequested();

            if (progress == null)
            {
                await download.CopyToAsync(destination, bufferSize, cancellationToken);
                return;
            }

            var bytesTotal = response.Content?.Headers?.ContentLength ?? 0L;
            var progressReporter = new Progress<long>(bytesDownloaded => progress.Report(new DownloadProgress
            {
                BytesDownloaded = bytesDownloaded,
                BytesTotal = bytesTotal
            }));
            await download.CopyToAsync(destination, progressReporter, cancellationToken, bufferSize);
        }
    }
}
