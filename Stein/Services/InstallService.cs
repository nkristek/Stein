using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBase.Extensions;

namespace Stein.Services
{
    public static class InstallService
    {
        private static string _InstallationLogFolderPath;
        public static string InstallationLogFolderPath
        {
            get
            {
                return _InstallationLogFolderPath;
            }

            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                _InstallationLogFolderPath = value;
            }
        }

        private static IEnumerable<InstalledProgram> _InstalledPrograms;

        public static IEnumerable<InstalledProgram> InstalledPrograms
        {
            get
            {
                if (_InstalledPrograms == null)
                    RefreshInstalledPrograms();
                return _InstalledPrograms;
            }

            private set
            {
                _InstalledPrograms?.ForEach(program => program.Dispose());
                _InstalledPrograms = value;
            }
        }

        public static void RefreshInstalledPrograms()
        {
            try
            {
                InstalledPrograms = ReadInstalledPrograms();
            }
            catch
            {
                InstalledPrograms = Enumerable.Empty<InstalledProgram>();
            }
        }

        public static async Task RefreshInstalledProgramsAsync()
        {
            await Task.Run(() =>
            {
                RefreshInstalledPrograms();
            });
        }

        private static IEnumerable<InstalledProgram> ReadInstalledPrograms()
        {
            const string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            
            using (var key = Registry.LocalMachine.OpenSubKey(registryPath))
                foreach (var program in GetInstalledProgramsFromKey(key))
                    yield return program;

            using (var key = Registry.CurrentUser.OpenSubKey(registryPath))
                foreach (var program in GetInstalledProgramsFromKey(key))
                    yield return program;

            const string wow6432NodeRegistryPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            using (var key = Registry.LocalMachine.OpenSubKey(wow6432NodeRegistryPath))
                foreach (var program in GetInstalledProgramsFromKey(key))
                    yield return program;
        }

        private static IEnumerable<InstalledProgram> GetInstalledProgramsFromKey(RegistryKey key)
        {
            foreach (var subkeyName in key.GetSubKeyNames())
                yield return new InstalledProgram
                {
                    RegistryKey = key.OpenSubKey(subkeyName)
                };
        }

        public static bool IsProductCodeInstalled(string productCode)
        {
            return !String.IsNullOrEmpty(productCode) && InstalledPrograms.Any(program => !String.IsNullOrEmpty(program.UninstallString) && program.UninstallString.Contains(productCode));
        }

        public static void Install(string installerPath, string logFilePath = null, bool quiet = true)
        {
            var process = StartInstallProcess(installerPath, logFilePath, quiet);
            process.WaitForExit();
        }

        public static async Task InstallAsync(string installerPath, string logFilePath = null, bool quiet = true)
        {
            var process = StartInstallProcess(installerPath, logFilePath, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        private static Process StartInstallProcess(string installerPath, string logFilePath = null, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installerPath))
                throw new ArgumentException("Installer path is null or empty");

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append(String.Format("/I \"{0}\"", installerPath));

            if (quiet)
                argumentsBuilder.Append(" /QN");

            if (!String.IsNullOrEmpty(logFilePath))
                argumentsBuilder.Append(String.Format(" /L*V \"{0}\"", logFilePath));

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString()
            };

            return Process.Start(startInfo);
        }
        
        public static void Reinstall(string productCodeToUninstall, string installerPathToInstall, string logFilePath = null, bool quiet = true)
        {
            // Note: reinstalling with a different installer fails, so for now uninstall and install it.
            var uninstallProcess = StartUninstallProcess(productCodeToUninstall, logFilePath, quiet);
            uninstallProcess.WaitForExit();

            var installProcess = StartInstallProcess(installerPathToInstall, logFilePath, quiet);
            installProcess.WaitForExit();
        }

        public static async Task ReinstallAsync(string productCodeToUninstall, string installerPathToInstall, string logFilePath = null, bool quiet = true)
        {
            // Note: reinstalling with a different installer fails, so for now uninstall and install it.
            var uninstallProcess = StartUninstallProcess(productCodeToUninstall, logFilePath, quiet);
            await uninstallProcess.WaitForExitAsync().ConfigureAwait(false);

            var installProcess = StartInstallProcess(installerPathToInstall, logFilePath, quiet);
            await installProcess.WaitForExitAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Caution, only use this with the original installer. Otherwise the reinstallation may fail.
        /// </summary>
        /// <param name="installerPath">Path to the installer</param>
        /// <param name="logFilePath">Path to the log file for the installation. (optional)</param>
        /// <param name="quiet">True for no UI. False for no UI.</param>
        /// <returns></returns>
        private static Process StartReinstallProcess(string installerPath, string logFilePath = null, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installerPath))
                throw new ArgumentException("Installer path is empty");

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append(String.Format("/FAMUS \"{0}\"", installerPath));

            if (quiet)
                argumentsBuilder.Append(" /QN");

            if (!String.IsNullOrEmpty(logFilePath))
                argumentsBuilder.Append(String.Format(" /L*V \"{0}\"", logFilePath));

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString()
            };

            return Process.Start(startInfo);
        }

        public static void Uninstall(string productCode, string logFilePath = null, bool quiet = true)
        {
            var process = StartUninstallProcess(productCode, logFilePath, quiet);
            process.WaitForExit();
        }

        public static async Task UninstallAsync(string productCode, string logFilePath = null, bool quiet = true)
        {
            var process = StartUninstallProcess(productCode, logFilePath, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        private static Process StartUninstallProcess(string productCode, string logFilePath = null, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(productCode))
                throw new ArgumentException("Productcode is empty");

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append(String.Format("/X {0}", productCode));

            if (quiet)
                argumentsBuilder.Append(" /QN");

            if (!String.IsNullOrEmpty(logFilePath))
                argumentsBuilder.Append(String.Format(" /L*V \"{0}\"", logFilePath));

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString(),
                UseShellExecute = false
            };

            return Process.Start(startInfo);
        }
    }
}
