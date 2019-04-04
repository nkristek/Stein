using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Stein.Helpers.Tests
{
    public class ProcessExtensionsTests
    {
        [Fact]
        public async Task WaitForExitAsync()
        {
            // start process
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c timeout 2",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            Assert.True(process.Start());

            // wait for the process to finish
            await process.WaitForExitAsync();
        }

        [Fact]
        public void WaitForExitAsync_CancellationToken()
        {
            // start process
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c timeout 1",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            Assert.True(process.Start());

            // wait on process with cancellation token
            var tokenSource = new CancellationTokenSource();
            var cancelTask = process.WaitForExitAsync(tokenSource.Token);
            Assert.False(cancelTask.IsCanceled, "The task is already cancelled.");
            Assert.False(cancelTask.IsCompleted, "The task is already completed.");
            Assert.False(cancelTask.IsFaulted, "The task is already faulted.");

            // cancel waiting
            tokenSource.Cancel();
            Assert.True(cancelTask.IsCanceled, "The task should be cancelled now.");

            // wait for the process to finish
            process.WaitForExit();
        }
    }
}
