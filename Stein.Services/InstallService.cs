using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Stein.Helpers;
using Stein.Localizations;
using Stein.Services.Types;

namespace Stein.Services
{
    public class InstallService
        : IInstallService
    {
        public static IInstallService Instance;

        private List<InstalledProgram> _installedPrograms;

        public IEnumerable<InstalledProgram> InstalledPrograms
        {
            get
            {
                if (_installedPrograms == null)
                    RefreshInstalledPrograms();
                return _installedPrograms;
            }

            private set
            {
                _installedPrograms?.ForEach(program => program.Dispose());
                _installedPrograms = value.ToList();
            }
        }
        
        public void RefreshInstalledPrograms()
        {
            InstalledPrograms = ReadInstalledPrograms();
        }
        
        private IEnumerable<InstalledProgram> ReadInstalledPrograms()
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
            return key.GetSubKeyNames().Select(subkeyName => new InstalledProgram
            {
                RegistryKey = key.OpenSubKey(subkeyName)
            });
        }
        
        public bool IsProductCodeInstalled(string productCode)
        {
            return !String.IsNullOrEmpty(productCode) && InstalledPrograms.Any(program => !String.IsNullOrEmpty(program.UninstallString) && program.UninstallString.Contains(productCode));
        }
        
        public void Install(string installerPath, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            var process = StartInstallProcess(installerPath, logFilePath, quiet, disableReboot);
            process.WaitForExit();
        }
        
        public async Task InstallAsync(string installerPath, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            var process = StartInstallProcess(installerPath, logFilePath, quiet, disableReboot);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }
        
        private static Process StartInstallProcess(string installerPath, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            if (String.IsNullOrWhiteSpace(installerPath))
                throw new ArgumentException(Strings.InstallerPathIsEmpty);

            if (!File.Exists(installerPath))
                throw new FileNotFoundException(String.Format(Strings.InstallerNotFound, installerPath), installerPath);

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append($"/I \"{installerPath}\"");

            if (quiet)
                argumentsBuilder.Append(" /QN");

            if (disableReboot)
                argumentsBuilder.Append(" /norestart");

            if (!String.IsNullOrEmpty(logFilePath))
                argumentsBuilder.Append($" /L*V \"{logFilePath}\"");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString()
            };

            return Process.Start(startInfo);
        }
        
        public void Reinstall(string installerPathToReinstall, string reinstallLogFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            var reinstallProcess = StartReinstallProcess(installerPathToReinstall, reinstallLogFilePath, quiet, disableReboot);
            reinstallProcess.WaitForExit();
        }
        
        public async Task ReinstallAsync(string installerPathToReinstall, string reinstallLogFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            // Note: reinstalling with a different installer fails, so for now uninstall and install it.
            var reinstallProcess = StartReinstallProcess(installerPathToReinstall, reinstallLogFilePath, quiet, disableReboot);
            await reinstallProcess.WaitForExitAsync().ConfigureAwait(false);
        }
        
        private static Process StartReinstallProcess(string installerPath, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            if (String.IsNullOrWhiteSpace(installerPath))
                throw new ArgumentException(Strings.InstallerPathIsEmpty);

            if (!File.Exists(installerPath))
                throw new FileNotFoundException(String.Format(Strings.InstallerNotFound, installerPath), installerPath);

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append($"/FAMUS \"{installerPath}\"");

            if (quiet)
                argumentsBuilder.Append(" /QN");

            if (disableReboot)
                argumentsBuilder.Append(" /norestart");

            if (!String.IsNullOrEmpty(logFilePath))
                argumentsBuilder.Append($" /L*V \"{logFilePath}\"");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString()
            };

            return Process.Start(startInfo);
        }
        
        public void Uninstall(string productCode, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            var process = StartUninstallProcess(productCode, logFilePath, quiet, disableReboot);
            process.WaitForExit();
        }
        
        public async Task UninstallAsync(string productCode, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            var process = StartUninstallProcess(productCode, logFilePath, quiet, disableReboot);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }
        
        private static Process StartUninstallProcess(string productCode, string logFilePath = null, bool quiet = true, bool disableReboot = true)
        {
            if (String.IsNullOrWhiteSpace(productCode))
                throw new ArgumentException(Strings.ProductCodeIsEmpty);

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append($"/X {productCode}");

            if (quiet)
                argumentsBuilder.Append(" /QN");

            if (disableReboot)
                argumentsBuilder.Append(" /norestart");

            if (!String.IsNullOrEmpty(logFilePath))
                argumentsBuilder.Append($" /L*V \"{logFilePath}\"");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString(),
                UseShellExecute = false
            };

            return Process.Start(startInfo);
        }
    }
}
