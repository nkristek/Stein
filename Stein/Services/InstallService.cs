using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.ViewModels;
using WpfBase.Extensions;

namespace Stein.Services
{
    public static class InstallService
    {
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

        public static void Install(InstallerViewModel installer, bool quiet = true)
        {
            var process = StartInstallProcess(installer, quiet);
            process.WaitForExit();
        }

        public static async Task InstallAsync(InstallerViewModel installer, bool quiet = true)
        {
            var process = StartInstallProcess(installer, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        private static Process StartInstallProcess(InstallerViewModel installer, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installer.Path))
                throw new ArgumentException("Installer path is empty");

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append("/I ");
            argumentsBuilder.Append(installer.Path.Quoted());

            if (quiet)
                argumentsBuilder.Append(" /QN");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString()
            };

            return Process.Start(startInfo);
        }

        public static void Reinstall(InstallerViewModel installer, bool quiet = true)
        {
            var process = StartInstallProcess(installer, quiet);
            process.WaitForExit();
        }

        public static async Task ReinstallAsync(InstallerViewModel installer, bool quiet = true)
        {
            var process = StartInstallProcess(installer, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        private static Process StartReinstallProcess(InstallerViewModel installer, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installer.Path))
                throw new ArgumentException("Installer path is empty");

            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append("/FAMUS ");
            
            if (!String.IsNullOrEmpty(installer.ProductCode))
                argumentsBuilder.Append(installer.ProductCode);
            else
                argumentsBuilder.Append(installer.Path.Quoted());

            if (quiet)
                argumentsBuilder.Append(" /QN");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString()
            };

            return Process.Start(startInfo);
        }

        public static void Uninstall(InstallerViewModel installer, bool quiet = true)
        {
            var process = StartUninstallProcess(installer, quiet);
            process.WaitForExit();
        }

        public static async Task UninstallAsync(InstallerViewModel installer, bool quiet = true)
        {
            var process = StartUninstallProcess(installer, quiet);
            await process.WaitForExitAsync().ConfigureAwait(false);
        }

        private static Process StartUninstallProcess(InstallerViewModel installer, bool quiet = true)
        {
            if (String.IsNullOrWhiteSpace(installer.Path))
                throw new ArgumentException("Installer path is empty");

            var argumentsBuilder = new StringBuilder();

            //if (quiet)
                argumentsBuilder.Append("/X ");
            //else
            //    argumentsBuilder.Append("/I ");
            
            if (!String.IsNullOrEmpty(installer.ProductCode))
                argumentsBuilder.Append(installer.ProductCode);
            else
                argumentsBuilder.Append(installer.Path.Quoted());

            if (quiet)
                argumentsBuilder.Append(" /QN");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString(),
                UseShellExecute = false
            };

            return Process.Start(startInfo);
        }
    }
}
