using System;
using System.Collections.Generic;
using System.Linq;
using Stein.ConfigurationTypes;
using Stein.ViewModels;
using WpfBase.ViewModels;

namespace Stein.Services
{
    public static class ViewModelService
    {
        public static IEnumerable<ApplicationViewModel> CreateOrUpdateApplicationViewModels(ViewModel parent = null, IEnumerable<ApplicationViewModel> existingApplicationViewModels = null)
        {
            foreach (var applicationFolder in ConfigurationService.Configuration.ApplicationFolders)
            {
                var existingApplicationViewModel = existingApplicationViewModels?.FirstOrDefault(avm => avm.FolderId == applicationFolder.Id);
                if (existingApplicationViewModel != null)
                {
                    UpdateApplicationViewModel(existingApplicationViewModel, applicationFolder);
                    yield return existingApplicationViewModel;
                }
                else
                {
                    yield return CreateApplicationViewModel(applicationFolder, parent);
                }
            }
        }

        public static void SaveViewModel<TViewModel>(TViewModel viewModelToSave) where TViewModel : ViewModel
        {
            if (!viewModelToSave.IsDirty)
                return;

            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                SaveApplicationViewModel(viewModelToSave as ApplicationViewModel);
                return;
            }
            throw new NotSupportedException("This ViewModel is not supported.");
        }

        private static void SaveApplicationViewModel(ApplicationViewModel application)
        {
            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
            if (associatedApplicationFolder == null)
                return;

            associatedApplicationFolder.Name = application.Name;
            associatedApplicationFolder.Path = application.Path;
            associatedApplicationFolder.EnableSilentInstallation = application.EnableSilentInstallation;

            foreach (var installerBundle in application.InstallerBundles.Where(installerBundle => installerBundle.IsDirty))
                SaveInstallerBundleViewModel(installerBundle, associatedApplicationFolder);

            application.IsDirty = false;
        }

        private static void SaveInstallerBundleViewModel(InstallerBundleViewModel installerBundle, ApplicationFolder parentApplicationFolder)
        {
            var associatedSubFolder = parentApplicationFolder.FindSubFolder(installerBundle.Path);
            if (associatedSubFolder == null)
                return;

            foreach (var installer in installerBundle.Installers.Where(installer => installer.IsDirty))
                SaveInstallerViewModel(installer, associatedSubFolder);
            
            installerBundle.IsDirty = false;
        }

        private static void SaveInstallerViewModel(InstallerViewModel installer, SubFolder parentSubFolder)
        {
            var installerFile = parentSubFolder.FindInstallerFile(installer.Path);
            if (installerFile == null)
                return;

            installerFile.IsEnabled = installer.IsEnabled;

            installer.IsDirty = false;
        }
        
        public static TViewModel CreateViewModel<TViewModel>(object entity, ViewModel parent = null) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                return CreateApplicationViewModel(entity as ApplicationFolder, parent) as TViewModel;
            throw new NotSupportedException("This ViewModel is not supported.");
        }
        
        private static ApplicationViewModel CreateApplicationViewModel(ApplicationFolder applicationFolder, ViewModel parent = null)
        {
            if (applicationFolder == null)
                throw new ArgumentNullException("applicationFolder");

            var application = new ApplicationViewModel(parent)
            {
                FolderId = applicationFolder.Id,
                Name = applicationFolder.Name,
                Path = applicationFolder.Path,
                EnableSilentInstallation = applicationFolder.EnableSilentInstallation
            };

            foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(applicationFolder, application))
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;

            return application;
        }

        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(ApplicationFolder applicationFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var subFolder in applicationFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        private static IEnumerable<InstallerFile> FindInstallerFiles(SubFolder folder)
        {
            foreach (var installerFile in folder.InstallerFiles)
                yield return installerFile;

            foreach (var subFolder in folder.SubFolders)
                foreach (var installerFile in FindInstallerFiles(subFolder))
                    yield return installerFile;
        }

        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(SubFolder subFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var installerFileGroup in FindInstallerFiles(subFolder).GroupBy(i => i.Culture))
            {
                var currentCulture = installerFileGroup.FirstOrDefault()?.Culture;

                var installerBundle = existingInstallerBundles?.FirstOrDefault(ib => ib.Path == subFolder.Path && ib.Culture == currentCulture);
                if (installerBundle == null)
                {
                    installerBundle = new InstallerBundleViewModel(parent)
                    {
                        Name = subFolder.Name,
                        Path = subFolder.Path
                    };
                }

                foreach (var installer in installerFileGroup)
                {
                    var installerViewModel = installerBundle.Installers.FirstOrDefault(i => i.Path == installer.Path);
                    if (installerViewModel == null)
                    {
                        installerViewModel = new InstallerViewModel(installerBundle);
                        installerBundle.Installers.Add(installerViewModel);
                    }

                    installerViewModel.Name = installer.Name;
                    installerViewModel.Path = installer.Path;
                    installerViewModel.IsEnabled = installer.IsEnabled;
                    installerViewModel.IsDisabled = false;
                    installerViewModel.Version = installer.Version;
                    installerViewModel.Culture = installer.Culture;
                    installerViewModel.ProductCode = installer.ProductCode;
                    installerViewModel.IsInstalled = InstallService.IsProductCodeInstalled(installer.ProductCode);
                    installerViewModel.Created = installer.Created;

                    installerViewModel.IsDirty = false;
                }

                installerBundle.IsDirty = false;

                yield return installerBundle;
            }

            foreach (var subSubFolder in subFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subSubFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        public static void UpdateViewModel<TViewModel>(TViewModel viewModelToUpdate, object entity = null) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                UpdateApplicationViewModel(viewModelToUpdate as ApplicationViewModel, entity as ApplicationFolder);
                return;
            }
            throw new NotSupportedException("This ViewModel is not supported.");
        }

        private static void UpdateApplicationViewModel(ApplicationViewModel application, ApplicationFolder associatedApplicationFolder = null)
        {
            if (associatedApplicationFolder == null)
            {
                associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
                if (associatedApplicationFolder == null)
                    throw new Exception("folder not found in cached folders");
            }

            application.FolderId = associatedApplicationFolder.Id;
            application.Name = associatedApplicationFolder.Name;
            application.Path = associatedApplicationFolder.Path;
            application.EnableSilentInstallation = associatedApplicationFolder.EnableSilentInstallation;

            var installerBundles = CreateOrUpdateInstallerBundleViewModels(associatedApplicationFolder, application, application.InstallerBundles.ToList());
            application.InstallerBundles.Clear();
            foreach (var installerBundle in installerBundles)
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;
        }
    }
}
