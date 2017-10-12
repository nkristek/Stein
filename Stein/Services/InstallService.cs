using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stein.ViewModels;
using WpfBase.Extensions;
using WindowsInstaller;

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
            _InstalledPrograms?.ForEach(program => program.Dispose());
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

        public static void Install(InstallerViewModel installer, bool quiet = true)
        {
            try
            {
                var process = StartInstallProcess(installer, quiet);
                process.WaitForExit();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static async Task InstallAsync(InstallerViewModel installer, bool quiet = true)
        {
            try
            {
                var process = StartInstallProcess(installer, quiet);
                await process.WaitForExitAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private static Process StartInstallProcess(InstallerViewModel installer, bool quiet = true)
        {
            var argumentsBuilder = new StringBuilder();

            argumentsBuilder.Append("/I ");
            argumentsBuilder.Append(installer.Path?.Quoted());

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
            try
            {
                var process = StartUninstallProcess(installer, quiet);
                process.WaitForExit();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static async Task UninstallAsync(InstallerViewModel installer, bool quiet = true)
        {
            try
            {
                var process = StartUninstallProcess(installer, quiet);
                await process.WaitForExitAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private static Process StartUninstallProcess(InstallerViewModel installer, bool quiet = true)
        {
            var argumentsBuilder = new StringBuilder();

            if (quiet)
                argumentsBuilder.Append("/X ");
            else
                argumentsBuilder.Append("/I ");

            if (installer.MsiProperties.ContainsKey("ProductCode") && !String.IsNullOrEmpty(installer.MsiProperties["ProductCode"]))
                argumentsBuilder.Append(installer.MsiProperties["ProductCode"]);
            else
                argumentsBuilder.Append(installer.Path?.Quoted());

            if (quiet)
                argumentsBuilder.Append(" /QN");

            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = argumentsBuilder.ToString(),
                UseShellExecute = false
            };

            return Process.Start(startInfo);
        }

        public static Dictionary<string, string> GetPropertiesFromMsi(string fileName)
        {
            // Get the type of the Windows Installer object 
            var installerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");

            // Create the Windows Installer object 
            var installer = Activator.CreateInstance(installerType) as Installer;

            // Open the MSI database in the input file 
            var database = installer.OpenDatabase(fileName, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);

            // Open a view on the Property table for the version property 
            var view = database.OpenView("SELECT * FROM Property");

            // Execute the view query 
            view.Execute(null);

            // Get the records from the view 
            var properties = new Dictionary<string, string>();

            var record = view.Fetch();
            while (record != null)
            {
                var key = record.get_StringData(1);
                var value = record.get_StringData(2);

                if (!String.IsNullOrEmpty(key) && !properties.ContainsKey(key))
                    properties.Add(key, value);

                record = view.Fetch();
            }

            return properties;
        }
    }
}
