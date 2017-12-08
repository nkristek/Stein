using Microsoft.Win32;
using Stein.Localizations;
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

        public static string GetLogFilePathForInstaller(string installerName)
        {
            var currentDate = DateTime.Now;
            var logFileName = String.Format("{0}_{1}-{2}-{3}_{4}-{5}-{6}.txt", installerName, currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
            return Path.Combine(InstallationLogFolderPath, logFileName);
        }

        private static List<InstalledProgram> _InstalledPrograms;
        /// <summary>
        /// List of installed programs read from the registry
        /// </summary>
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
                _InstalledPrograms = value.ToList();
            }
        }

        /// <summary>
        /// Refreshes the list of installed programs
        /// </summary>
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

        /// <summary>
        /// Refreshes the list of installed programs asynchronously
        /// </summary>
        /// <returns>Task which refreshes the list of installed programs</returns>
        public static async Task RefreshInstalledProgramsAsync()
        {
            await Task.Run(() =>
            {
                RefreshInstalledPrograms();
            });
        }

        /// <summary>
        /// Reads the registry for installed programs
        /// </summary>
        /// <returns>List of installed programs</returns>
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

        /// <summary>
        /// Reads all installed programs from a registry key
        /// </summary>
        /// <param name="key">RegistryKey to the registry path in which the installed programs are listed</param>
        /// <returns>List of installed programs</returns>
        private static IEnumerable<InstalledProgram> GetInstalledProgramsFromKey(RegistryKey key)
        {
            foreach (var subkeyName in key.GetSubKeyNames())
                yield return new InstalledProgram
                {
                    RegistryKey = key.OpenSubKey(subkeyName)
                };
        }

        /// <summary>
        /// Searches in the installed programs if a product code is installed
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public static bool IsProductCodeInstalled(string productCode)
        {
            return !String.IsNullOrEmpty(productCode) && InstalledPrograms.Any(program => !String.IsNullOrEmpty(program.UninstallString) && program.UninstallString.Contains(productCode));
        }

        /// <summary>
        /// Installs a installer file
        /// </summary>
        /// <param name="installerPath">Path to the installer file</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be installed without UI</param>
        public static void Install(string installerPath, string logFilePath = null, bool quiet = true)
        {
            var process = StartInstallProcess(installerPath, logFilePath, quiet);
            process.WaitForExit();
        }

        /// <summary>
        /// Installs a installer file asynchronously
        /// </summary>
        /// <param name="installerPath">Path to the installer file</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be installed without UI</param>
        /// <returns>Task which installs a installer file</returns>
        public static async Task InstallAsync(string installerPath, string logFilePath = null, bool quiet = true)
        {
            var process = StartInstallProcess(installerPath, logFilePath, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a process for installing a installer file
        /// </summary>
        /// <param name="installerPath">Path to the installer file</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be installed without UI</param>
        /// <returns>Process of the installer</returns>
        private static Process StartInstallProcess(string installerPath, string logFilePath = null, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installerPath))
                throw new ArgumentException(Strings.InstallerPathIsEmpty);

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

        /// <summary>
        /// Uninstalls and installs a installer file
        /// Warning: reinstalling with a different installer fails, so for now uninstall and install it.
        /// </summary>
        /// <param name="installerPathToInstall">Path to the installer file</param>
        /// <param name="reinstallLogFilePath">Path to a log file for uninstalling (optional)</param>
        /// <param name="quiet">If it should be reinstalled without UI</param>
        public static void Reinstall(string installerPathToReinstall, string reinstallLogFilePath = null, bool quiet = true)
        {
            var reinstallProcess = StartReinstallProcess(installerPathToReinstall, reinstallLogFilePath, quiet);
            reinstallProcess.WaitForExit();
        }

        /// <summary>
        /// Uninstalls and installs a installer file asynchronously
        /// Warning: reinstalling with a different installer fails, so for now uninstall and install it.
        /// </summary>
        /// <param name="installerPathToInstall">Path to the installer file</param>
        /// <param name="reinstallLogFilePath">Path to a log file for uninstalling (optional)</param>
        /// <param name="quiet">If it should be reinstalled without UI</param>
        /// <returns>Task which uninstalls and installs a installer file</returns>
        public static async Task ReinstallAsync(string installerPathToReinstall, string reinstallLogFilePath = null, bool quiet = true)
        {
            // Note: reinstalling with a different installer fails, so for now uninstall and install it.
            var reinstallProcess = StartReinstallProcess(installerPathToReinstall, reinstallLogFilePath, quiet);
            await reinstallProcess.WaitForExitAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Caution, only use this with the original installer. Otherwise the reinstallation may fail.
        /// </summary>
        /// <param name="installerPath">Path to the installer</param>
        /// <param name="logFilePath">Path to the log file for the installation. (optional)</param>
        /// <param name="quiet">If it should be reinstalled without UI</param>
        /// <returns></returns>
        private static Process StartReinstallProcess(string installerPath, string logFilePath = null, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installerPath))
                throw new ArgumentException(Strings.InstallerPathIsEmpty);

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

        /// <summary>
        /// Uninstalls a program
        /// </summary>
        /// <param name="productCode">Product code of the installed program</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be uninstalled without UI</param>
        public static void Uninstall(string productCode, string logFilePath = null, bool quiet = true)
        {
            var process = StartUninstallProcess(productCode, logFilePath, quiet);
            process.WaitForExit();
        }

        /// <summary>
        /// Uninstalls a program asynchronously
        /// </summary>
        /// <param name="productCode">Product code of the installed program</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be uninstalled without UI</param>
        /// <returns>Task which uninstalls a installer file</returns>
        public static async Task UninstallAsync(string productCode, string logFilePath = null, bool quiet = true)
        {
            var process = StartUninstallProcess(productCode, logFilePath, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a process for uninstalling a product code
        /// </summary>
        /// <param name="productCode">Product code of the installed program</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be uninstalled without UI</param>
        /// <returns>Process of the uninstaller</returns>
        private static Process StartUninstallProcess(string productCode, string logFilePath = null, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(productCode))
                throw new ArgumentException(Strings.ProductCodeIsEmpty);

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
