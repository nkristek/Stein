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

namespace Stein.Services
{
    public static class ViewModelService
    {
        private static readonly IReadOnlyDictionary<Type, Type> ViewModelsToViewsMapping = new Dictionary<Type, Type>
        {
            { typeof(InstallerBundleViewModel), typeof(SelectInstallersDialog) }
        };

        public static bool? ShowDialog(ViewModel dialogViewModel, string title = null)
        {
            var viewModelType = dialogViewModel.GetType();
            if (!ViewModelsToViewsMapping.ContainsKey(viewModelType))
                throw new NotSupportedException("No view found for viewmodel");
            
            var dialog = Activator.CreateInstance(ViewModelsToViewsMapping[viewModelType]) as Window;
            if (dialog == null)
                throw new ArgumentException("view for viewmodel is no window");

            dialog.Title = title ?? String.Empty;
            dialog.DataContext = dialogViewModel;
            dialog.Owner = dialogViewModel.Parent?.View as Window;

            dialogViewModel.View = dialog;

            return dialog.ShowDialog();
        }

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

            foreach (var installerBundle in GetInstallerBundlesFromApplication(application))
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            return application;
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

                var msiDatabase = InstallService.GetMsiDatabase(fileName);

                yield return new InstallerViewModel()
                {
                    Path = fileName,
                    IsEnabled = true,
                    IsDisabled = false
                };
            }
        }
    }
}
