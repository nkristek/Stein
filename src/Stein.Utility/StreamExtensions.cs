using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Utility
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Asynchronously copy data from the <paramref name="source"/> <see cref="Stream"/> to the <paramref name="destination"/> <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Stream"/>.</param>
        /// <param name="destination">Destination <see cref="Stream"/>.</param>
        /// <param name="progress">Progress reporter.</param>
        /// <param name="bufferSize">Size of the buffer to use.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>Asynchronous <see cref="Task"/> which copies the data.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="destination"/> or <paramref name="progress"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/> can't read or <paramref name="destination"/> can't write.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative.</exception>
        public static async Task CopyToAsync(this Stream source, Stream destination, IProgress<long> progress, int bufferSize = 81920, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            var totalBytesRead = 0L;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
            await destination.FlushAsync(cancellationToken);
        }
    }
}
