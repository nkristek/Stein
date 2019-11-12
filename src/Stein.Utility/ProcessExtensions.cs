using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Utility
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Waits asynchronously for the <see cref="Process"/> to exit.
        /// https://stackoverflow.com/a/50461641
        /// </summary>
        /// <param name="process">The <see cref="Process"/> to wait on.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/>. If invoked, the task will return immediately as canceled.</param>
        /// <returns>A <see cref="Task"/> representing waiting for the <see cref="Process"/> to end.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="process"/> is <see langword="null"/>.</exception>
        public static async Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            var tcs = new TaskCompletionSource<bool>();

            void ProcessExited(object? sender, EventArgs? e) => tcs.TrySetResult(true);
            process.EnableRaisingEvents = true;
            process.Exited += ProcessExited;

            try
            {
                if (process.HasExited)
                    return;

                using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                    await tcs.Task;
            }
            finally
            {
                process.Exited -= ProcessExited;
            }
        }
    }
}
