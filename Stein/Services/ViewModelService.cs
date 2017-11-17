using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stein.Configuration;
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
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                SaveApplicationViewModel(viewModelToSave as ApplicationViewModel);
                return;
            }
            throw new NotSupportedException("This ViewModel is not supported.");
        }

        private static void SaveApplicationViewModel(ApplicationViewModel applicationViewModelToSave)
        {
            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == applicationViewModelToSave.FolderId);
            if (associatedApplicationFolder == null)
                return;

            foreach (var installerBundle in applicationViewModelToSave.InstallerBundles)
            {
                foreach (var installer in installerBundle.Installers)
                {
                    var installerFile = FindInstallerFileInApplicationFolder(associatedApplicationFolder, installer.Path);
                    if (installerFile == null)
                        continue;

                    installerFile.Created = installer.Created ?? DateTime.MinValue;
                    installerFile.Culture = installer.Culture;
                    installerFile.IsEnabled = installer.IsEnabled;
                    installerFile.Name = installer.Name;
                    installerFile.Path = installer.Path;
                    installerFile.ProductCode = installer.ProductCode;
                    installerFile.Version = installer.Version;

                    installer.IsDirty = false;
                }
                installerBundle.IsDirty = false;
            }
            applicationViewModelToSave.IsDirty = false;
        }

        private static InstallerFile FindInstallerFileInApplicationFolder(ApplicationFolder applicationFolder, string installerFilePath)
        {
            var installerFileRelativePath = installerFilePath.TrimStart(applicationFolder.Path.ToCharArray()).Split('\\');
            return FindInstallerFileInApplicationFolder(applicationFolder, installerFileRelativePath);
        }

        private static InstallerFile FindInstallerFileInApplicationFolder(ApplicationFolder applicationFolder, IEnumerable<string> installerFilePath)
        {
            var folderName = installerFilePath.FirstOrDefault();
            if (folderName == null)
                return null;

            var subFolder = applicationFolder.SubFolders.FirstOrDefault(sf => sf.Name == folderName);
            if (subFolder == null)
                return null;
            
            return FindInstallerFileInSubFolder(subFolder, installerFilePath.Skip(1));
        }

        private static InstallerFile FindInstallerFileInSubFolder(SubFolder subFolder, IEnumerable<string> installerFilePath)
        {
            if (installerFilePath.Count() == 1)
            {
                var installerFileName = installerFilePath.FirstOrDefault();
                return subFolder.InstallerFiles.FirstOrDefault(i => Path.GetFileName(i.Path) == installerFileName);
            }

            var folderName = installerFilePath.FirstOrDefault();
            if (folderName == null)
                return null;

            var subSubFolder = subFolder.SubFolders.FirstOrDefault(sf => sf.Name == folderName);
            if (subSubFolder == null)
                return null;

            return FindInstallerFileInSubFolder(subFolder, installerFilePath.Skip(1));
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

            if (application.SelectedInstallerBundle == null || !application.InstallerBundles.Any(ib => ib == application.SelectedInstallerBundle))
                application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;
        }
        
        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(ApplicationFolder applicationFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var subFolder in applicationFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(SubFolder subFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var installerFileGroup in subFolder.InstallerFiles.GroupBy(i => i.Culture))
            {
                var installerBundle = existingInstallerBundles?.FirstOrDefault(ib => ib.Name == subFolder.Name &&  ib.Path == subFolder.Path && ib.Culture == installerFileGroup.FirstOrDefault()?.Culture);
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
    }
}
