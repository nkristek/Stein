using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.Configuration;
using Stein.ViewModels;
using WindowsInstaller;
using WpfBase.ViewModels;
using System.Windows;
using Stein.Views;
using System.Threading;

namespace Stein.Services
{
    public static class ViewModelService
    {
        public static IEnumerable<ApplicationViewModel> CreateApplicationViewModels(ViewModel parent = null)
        {
            foreach (var setup in AppConfigurationService.CurrentConfiguration.Setups)
                yield return CreateApplicationViewModel(setup, parent);
        }

        public static ApplicationViewModel CreateApplicationViewModel(SetupConfiguration setup, ViewModel parent = null)
        {
            var application = new ApplicationViewModel(parent)
            {
                Name = setup.Name,
                Path = setup.Path,
                EnableSilentInstallation = setup.EnableSilentInstallation,
                AssociatedSetup = setup
            };

            IEnumerable<InstallerBundleViewModel> installerBundles;
            try
            {
                installerBundles = GetInstallerBundlesFromApplication(application);
            }
            catch
            {
                installerBundles = Enumerable.Empty<InstallerBundleViewModel>();
            }

            foreach (var installerBundle in installerBundles)
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            return application;
        }
        
        public static IEnumerable<InstallerBundleViewModel> GetInstallerBundlesFromApplication(ApplicationViewModel application)
        {
            if (!Directory.Exists(application.Path))
                yield break;

            foreach (var directoryName in Directory.EnumerateDirectories(application.Path))
            {
                IEnumerable<InstallerViewModel> installers;
                try
                {
                    installers = GetInstallersFromDirectory(directoryName);
                } catch { continue; }

                foreach (var installerGroup in installers.GroupBy(i => i.Culture))
                {
                    var installerBundle = new InstallerBundleViewModel(application)
                    {
                        Path = directoryName
                    };

                    try
                    {
                        installerBundle.Name = new DirectoryInfo(directoryName).Name;
                    } catch { }

                    foreach (var installer in installerGroup)
                    {
                        installer.Parent = installerBundle;
                        installerBundle.Installers.Add(installer);
                    }

                    yield return installerBundle;
                }
            }
        }

        private const string installerFileNamePattern = "*.msi";

        public static IEnumerable<InstallerViewModel> GetInstallersFromDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                yield break;

            foreach (var fileName in Directory.EnumerateFiles(directoryPath, installerFileNamePattern))
            {
                var installer = new InstallerViewModel()
                {
                    Path = fileName,
                    IsEnabled = true,
                    IsDisabled = false
                };

                ReadMsiProperties(installer);

                yield return installer;
            }
        }

        private static void ReadMsiProperties(InstallerViewModel installer)
        {
            try
            {
                var database = InstallService.GetMsiDatabase(installer.Path);

                installer.Name = InstallService.GetPropertyFromMsiDatabase(database, InstallService.MsiPropertyName.ProductName);
                installer.Version = InstallService.GetVersionFromMsiDatabase(database);
                installer.Culture = InstallService.GetCultureTagFromMsiDatabase(database);
                installer.IsInstalled = InstallService.IsMsiInstalled(database);

                installer.Created = new FileInfo(installer.Path).CreationTime;
            } catch { }
        }
    }
}
