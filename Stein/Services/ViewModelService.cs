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
            foreach (var applicationFolder in ConfigurationService.Configuration.ApplicationFolders)
                yield return CreateApplicationViewModel(applicationFolder, parent);
        }

        public static void UpdateApplicationViewModel(ApplicationViewModel application)
        {
            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
            if (associatedApplicationFolder == null)
                return;

            application.FolderId = associatedApplicationFolder.Id;
            application.Name = associatedApplicationFolder.Name;
            application.Path = associatedApplicationFolder.Path;
            application.EnableSilentInstallation = associatedApplicationFolder.EnableSilentInstallation;

            foreach (var installerBundle in GetInstallerBundlesFromApplicationFolder(associatedApplicationFolder, application))
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();
        }

        public static ApplicationViewModel CreateApplicationViewModel(ApplicationFolder applicationFolder, ViewModel parent = null)
        {
            var application = new ApplicationViewModel(parent)
            {
                FolderId = applicationFolder.Id,
                Name = applicationFolder.Name,
                Path = applicationFolder.Path,
                EnableSilentInstallation = applicationFolder.EnableSilentInstallation
            };

            foreach (var installerBundle in GetInstallerBundlesFromApplicationFolder(applicationFolder, application))
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            return application;
        }

        public static IEnumerable<InstallerBundleViewModel> GetInstallerBundlesFromApplicationFolder(ApplicationFolder applicationFolder, ViewModel parent = null)
        {
            foreach (var subFolder in applicationFolder.SubFolders)
                foreach (var installerBundle in GetInstallerBundlesFromSubFolder(subFolder, parent))
                    yield return installerBundle;
        }

        public static IEnumerable<InstallerBundleViewModel> GetInstallerBundlesFromSubFolder(SubFolder subFolder, ViewModel parent = null)
        {
            if (subFolder.InstallerFiles.Any())
            {
                foreach (var installerFileGroup in subFolder.InstallerFiles.GroupBy(i => i.Culture))
                {
                    var installerBundle = new InstallerBundleViewModel(parent)
                    {
                        Name = subFolder.Name,
                        Path = subFolder.Path
                    };

                    foreach (var installer in installerFileGroup)
                    {
                        installerBundle.Installers.Add(new InstallerViewModel(installerBundle)
                        {
                            Name = installer.Name,
                            Path = installer.Path,
                            IsEnabled = true,
                            IsDisabled = false,
                            Version = installer.Version,
                            Culture = installer.Culture,
                            IsInstalled = InstallService.IsProductCodeInstalled(installer.ProductCode),
                            Created = installer.Created
                        });
                    }

                    yield return installerBundle;
                }
            }

            foreach (var subSubFolder in subFolder.SubFolders)
                foreach (var installerBundle in GetInstallerBundlesFromSubFolder(subSubFolder, parent))
                    yield return installerBundle;
        }
    }
}
