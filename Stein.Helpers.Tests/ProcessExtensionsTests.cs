using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Helpers.Tests
{
    [TestClass]
    public class ProcessExtensionsTests
    {
        [TestMethod]
        public async Task TestWaitForExitAsync()
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
            Assert.IsTrue(process.Start());

            // wait for the process to finish
            await process.WaitForExitAsync();
        }

        [TestMethod]
        public void TestWaitForExitAsyncCancellationToken()
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
            Assert.IsTrue(process.Start());

            // wait on process with cancellation token
            var tokenSource = new CancellationTokenSource();
            var cancelTask = process.WaitForExitAsync(tokenSource.Token);
            Assert.IsFalse(cancelTask.IsCanceled, "The task is already cancelled.");
            Assert.IsFalse(cancelTask.IsCompleted, "The task is already completed.");
            Assert.IsFalse(cancelTask.IsFaulted, "The task is already faulted.");

            // cancel waiting
            tokenSource.Cancel();
            Assert.IsTrue(cancelTask.IsCanceled, "The task should be cancelled now.");

            // wait for the process to finish
            process.WaitForExit();
        }
    }
}
