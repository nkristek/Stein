using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TempManager.ViewModels;

namespace TempManager.Services
{
    public static class InstallService
    {
        private static IEnumerable<InstalledProgram> _InstalledPrograms;

        public static IEnumerable<InstalledProgram> InstalledPrograms
        {
            get
            {
                if (_InstalledPrograms == null)
                    _InstalledPrograms = ReadInstalledPrograms();
                return _InstalledPrograms;
            }

            private set
            {
                _InstalledPrograms = value;
            }
        }

        public static void RefreshInstalledPrograms()
        {
            if (_InstalledPrograms != null)
                foreach (var installedProgram in _InstalledPrograms)
                    installedProgram.Dispose();
            _InstalledPrograms = ReadInstalledPrograms();
        }

        private static IEnumerable<InstalledProgram> ReadInstalledPrograms()
        {
            const string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            
            using (var key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                foreach (var program in GetInstalledProgramsFromKey(key))
                    yield return program;
            }

            using (var key = Registry.CurrentUser.OpenSubKey(registryPath))
            {
                foreach (var program in GetInstalledProgramsFromKey(key))
                    yield return program;
            }

            const string wow6432NodeRegistryPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            using (var key = Registry.LocalMachine.OpenSubKey(wow6432NodeRegistryPath))
            {
                foreach (var program in GetInstalledProgramsFromKey(key))
                    yield return program;
            }
        }

        private static IEnumerable<InstalledProgram> GetInstalledProgramsFromKey(RegistryKey key)
        {
            foreach (var subkeyName in key.GetSubKeyNames())
            {
                yield return new InstalledProgram
                {
                    RegistryKey = key.OpenSubKey(subkeyName)
                };
            }
        }

        public static async Task Install(InstallerViewModel installer)
        {
            try
            {
                var startInfo = new ProcessStartInfo("msiexec.exe")
                {
                    Arguments = String.Format("/I \"{0}\"", installer.Path)
                };

                var process = Process.Start(startInfo);
                await process.WaitForExitAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static async Task Uninstall(InstallerViewModel installer)
        {
            try
            {
                if (installer.InstalledProgram?.UninstallString == null || String.IsNullOrEmpty(installer.InstalledProgram.UninstallString))
                    return;

                var uninstallString = installer.InstalledProgram.UninstallString.Split(new[] { ' ' }, 2);
                if (!uninstallString.Any())
                    return;

                var startInfo = new ProcessStartInfo(uninstallString[0])
                {
                    Arguments = uninstallString.Count() > 1 ? string.Join(" ", uninstallString[1]) : null,
                    UseShellExecute = false
                };

                var process = Process.Start(startInfo);
                await process.WaitForExitAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
        
        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// https://stackoverflow.com/a/19104345
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return 
        /// immediately as canceled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(this Process process,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
                cancellationToken.Register(tcs.SetCanceled);

            return tcs.Task;
        }
    }
}
