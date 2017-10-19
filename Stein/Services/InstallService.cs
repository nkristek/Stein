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
using System.Globalization;

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
                _InstalledPrograms?.ForEach(program => program.Dispose());
                _InstalledPrograms = value;
            }
        }

        public static void RefreshInstalledPrograms()
        {
            InstalledPrograms = ReadInstalledPrograms();
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

            var productCode = GetPropertyFromMsi(installer.Path, MsiPropertyName.ProductCode);
            if (!String.IsNullOrEmpty(productCode))
                argumentsBuilder.Append(productCode);
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

        public enum MsiPropertyName
        {
            ProductLanguage,
            ProductVersion,
            ProductCode,
            ProductName,
            Manufacturer
        }

        public static Dictionary<string, string> GetPropertiesFromMsi(string fileName)
        {
            return GetPropertiesFromMsiDatabase(GetMsiDatabase(fileName));
        }

        public static Dictionary<string, string> GetPropertiesFromMsiDatabase(Database database)
        {
            var view = database.OpenView("SELECT * FROM Property");
            view.Execute(null);

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

        public static string GetPropertyFromMsi(string fileName, MsiPropertyName propertyName)
        {
            return GetPropertyFromMsiDatabase(GetMsiDatabase(fileName), propertyName);
        }

        public static string GetPropertyFromMsiDatabase(Database database, MsiPropertyName propertyName)
        {
            var query = String.Format("SELECT * FROM Property WHERE Property='{0}'", propertyName.ToString());
            var view = database.OpenView(query);
            view.Execute(null);

            var record = view.Fetch();
            return record?.get_StringData(2);
        }

        public static Database GetMsiDatabase(string fileName)
        {
            var installerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            var installer = Activator.CreateInstance(installerType) as Installer;
            return installer.OpenDatabase(fileName, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
        }

        /*
         * https://msdn.microsoft.com/en-us/library/windows/desktop/aa370905(v=vs.85).aspx
         */

        public static bool IsMsiInstalled(string fileName)
        {
            var msiDatabase = GetMsiDatabase(fileName);
            return IsMsiInstalled(msiDatabase);
        }

        public static bool IsMsiInstalled(Database msiDatabase)
        {
            var productCode = GetPropertyFromMsiDatabase(msiDatabase, MsiPropertyName.ProductCode);

            return InstalledPrograms.Any(p =>
            {
                return !String.IsNullOrEmpty(p.UninstallString) && p.UninstallString.Contains(productCode);
            });
        }

        public static string GetCultureTagFromMsi(string fileName)
        {
            var msiDatabase = GetMsiDatabase(fileName);
            return GetCultureTagFromMsiDatabase(msiDatabase);
        }

        public static string GetCultureTagFromMsiDatabase(Database msiDatabase)
        {
            try
            {
                var cultureIdProperty = GetPropertyFromMsiDatabase(msiDatabase, MsiPropertyName.ProductLanguage);
                if (String.IsNullOrEmpty(cultureIdProperty))
                    return null;

                var cultureId = int.Parse(cultureIdProperty);
                var culture = new CultureInfo(cultureId);
                return culture.IetfLanguageTag;
            }
            catch
            {
                return null;
            }
        }

        public static Version GetVersionFromMsi(string fileName)
        {
            var msiDatabase = GetMsiDatabase(fileName);
            return GetVersionFromMsiDatabase(msiDatabase);
        }

        public static Version GetVersionFromMsiDatabase(Database msiDatabase)
        {
            try
            {
                var versionProperty = GetPropertyFromMsiDatabase(msiDatabase, MsiPropertyName.ProductVersion);
                if (String.IsNullOrEmpty(versionProperty))
                    return null;

                return new Version(versionProperty);
            }
            catch
            {
                return null;
            }
        }
    }
}
