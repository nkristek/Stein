using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempManager.Configuration;
using TempManager.ViewModels;
using WindowsInstaller;
using WpfBase.ViewModels;

namespace TempManager.Services
{
    public static class ViewModelService
    {
        public static ApplicationViewModel CreateApplicationViewModel(SetupConfiguration setup, ViewModel parent = null)
        {
            var application = new ApplicationViewModel(parent)
            {
                Name = new DirectoryInfo(setup.Path).Name,
                Path = setup.Path
            };

            foreach (var installerBundle in GetInstallerBundlesFromApplication(application))
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            return application;
        }

        public static IEnumerable<ApplicationViewModel> CreateApplicationViewModels(ViewModel parent = null)
        {
            foreach (var setup in AppConfigurationService.CurrentConfiguration.Setups)
                yield return CreateApplicationViewModel(setup, parent);
        }

        public static IEnumerable<InstallerBundleViewModel> GetInstallerBundlesFromApplication(ApplicationViewModel application)
        {
            foreach (var directoryFullName in Directory.GetDirectories(application.Path))
            {
                foreach (var installerGroup in GetInstallersFromPath(directoryFullName).GroupBy(i => i.Culture))
                {
                    var installerBundle = new InstallerBundleViewModel(application)
                    {
                        Name = new DirectoryInfo(directoryFullName).Name,
                        Path = directoryFullName
                    };

                    foreach (var installer in installerGroup)
                    {
                        installer.Parent = installerBundle;
                        installerBundle.Installers.Add(installer);
                    }

                    yield return installerBundle;
                }
            }
        }

        public static IEnumerable<InstallerViewModel> GetInstallersFromPath(string path)
        {
            foreach (var fileName in Directory.GetFiles(path))
            {
                if (Path.GetExtension(fileName) != ".msi")
                    continue;

                var properties = GetPropertiesFromMsi(fileName);

#if(DEBUG)
                var pathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var debugFolderName = "TempManager_Debug";
                var debugFolderFullName = Path.Combine(pathToDesktop, debugFolderName);
                if (!Directory.Exists(debugFolderFullName))
                    Directory.CreateDirectory(debugFolderFullName);
                var debugOutputFileName = String.Join(String.Empty, Path.GetFileName(fileName), ".", Guid.NewGuid().ToString(), ".txt");
                var pathToFile = Path.Combine(debugFolderFullName, debugOutputFileName);
                using (var fileStream = new StreamWriter(pathToFile))
                {
                    foreach (var kvPair in properties)
                    {
                        fileStream.WriteLine(kvPair.Key);
                        fileStream.WriteLine(kvPair.Value);
                        fileStream.WriteLine("-------------------");
                    }
                }
#endif
                var installedProgram = GetInstalledProgram(properties);

                yield return new InstallerViewModel()
                {
                    Name = Path.GetFileName(fileName),
                    Path = fileName,
                    MsiProperties = properties,
                    InstalledProgram = installedProgram,
                    Culture = GetCultureFromMsiProperties(properties),
                    Version = GetVersionFromMsiProperties(properties),
                    IsInstalled = installedProgram != null
                };
            }
        }

        /*
         * https://msdn.microsoft.com/en-us/library/windows/desktop/aa370905(v=vs.85).aspx
         */

        private static string GetCultureFromMsiProperties(Dictionary<string, string> properties)
        {
            if (!properties.ContainsKey("ProductLanguage"))
                return null;

            try
            {
                var cultureId = int.Parse(properties["ProductLanguage"]);
                var culture = new CultureInfo(cultureId);
                return culture.IetfLanguageTag;
            }
            catch
            {
                return null;
            }
        }

        private static Version GetVersionFromMsiProperties(Dictionary<string, string> properties)
        {
            if (!properties.ContainsKey("ProductVersion"))
                return null;

            try
            {
                return new Version(properties["ProductVersion"]);
            }
            catch
            {
                return null;
            }
        }

        private static InstalledProgram GetInstalledProgram(Dictionary<string, string> properties)
        {
            return InstallService.InstalledPrograms.FirstOrDefault(p =>
            {
                var found = false;
                if (!String.IsNullOrEmpty(p.DisplayName) && properties.ContainsKey("ProductName"))
                {
                    if (p.DisplayName == properties["ProductName"])
                        found = true;
                    else
                        found = false;
                }

                if (found && !String.IsNullOrEmpty(p.DisplayVersion) && properties.ContainsKey("ProductVersion"))
                    if (p.DisplayVersion != properties["ProductVersion"])
                        found = false;

                if (found && !String.IsNullOrEmpty(p.Publisher) && properties.ContainsKey("Manufacturer"))
                    if (p.Publisher != properties["Manufacturer"])
                        found = false;

                return found;
            });
        }

        private static Dictionary<string, string> GetPropertiesFromMsi(string fileName)
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
