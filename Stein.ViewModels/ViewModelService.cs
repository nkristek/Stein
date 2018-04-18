using System;
using System.Collections.Generic;
using System.Linq;
using nkristek.MVVMBase.ViewModels;
using Stein.Localizations;
using Stein.Services.Extensions;
using Stein.Types.ConfigurationTypes;
using Stein.ViewModels;
using Stein.ViewModels.Types;

namespace Stein.Services
{
    public class ViewModelService
        : IViewModelService
    {
        public static IViewModelService Instance;

        public TViewModel CreateViewModel<TViewModel>(ViewModel parent = null) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                return CreateApplicationViewModel(parent) as TViewModel;
            if (typeof(TViewModel) == typeof(ApplicationDialogModel))
                return CreateApplicationDialogModel(parent) as TViewModel;
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        private static ApplicationViewModel CreateApplicationViewModel(ViewModel parent, ApplicationFolder applicationFolder = null)
        {
            if (applicationFolder == null)
            {
                applicationFolder = new ApplicationFolder
                {
                    Id = Guid.NewGuid()
                };
                ConfigurationService.Instance.Configuration.ApplicationFolders.Add(applicationFolder);
            }

            var application = new ApplicationViewModel
            {
                Parent = parent,
                FolderId = applicationFolder.Id,
                Name = applicationFolder.Name,
                Path = applicationFolder.Path,
                EnableSilentInstallation = applicationFolder.EnableSilentInstallation,
                DisableReboot = applicationFolder.DisableReboot,
                EnableInstallationLogging = applicationFolder.EnableInstallationLogging
            };
            var installerBundles = CreateOrUpdateInstallerBundleViewModels(applicationFolder, application).ToList();
            application.InstallerBundles.Clear();
            foreach (var installerBundle in installerBundles)
                application.InstallerBundles.Add(installerBundle);
            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;
            return application;
        }

        private static ApplicationDialogModel CreateApplicationDialogModel(ViewModel parent)
        {
            if (parent is ApplicationViewModel application)
            {
                return new ApplicationDialogModel
                {
                    Parent = application,
                    Title = Strings.EditFolder,
                    FolderId = application.FolderId,
                    Name = application.Name,
                    Path = application.Path,
                    EnableSilentInstallation = application.EnableSilentInstallation,
                    DisableReboot = application.DisableReboot,
                    EnableInstallationLogging = application.EnableInstallationLogging
                };
            }
            return new ApplicationDialogModel
            {
                Parent = parent,
                Title = Strings.AddFolder,
                EnableSilentInstallation = true,
                DisableReboot = true,
                EnableInstallationLogging = true
            };
        }

        public IEnumerable<TViewModel> CreateViewModels<TViewModel>(ViewModel parent = null, IEnumerable<TViewModel> existingViewModels = null) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                return CreateApplicationViewModels(parent, existingViewModels as IEnumerable<ApplicationViewModel>) as IEnumerable<TViewModel>;
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        private static IEnumerable<ApplicationViewModel> CreateApplicationViewModels(ViewModel parent, IEnumerable<ApplicationViewModel> existingViewModels)
        {
            var existingApplicationViewModels = existingViewModels.ToList();
            foreach (var applicationFolder in ConfigurationService.Instance.Configuration.ApplicationFolders)
            {
                var existingViewModel = existingApplicationViewModels.FirstOrDefault(avm => avm.FolderId == applicationFolder.Id);
                if (existingViewModel != null)
                {
                    UpdateApplicationViewModel(existingViewModel, applicationFolder);
                    existingViewModel.Parent = parent;
                    yield return existingViewModel;
                }
                else
                {
                    yield return CreateApplicationViewModel(parent, applicationFolder);
                }
            }
        }

        public void SaveViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
        {
            if (!viewModel.IsDirty)
                return;

            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                SaveApplicationViewModel(viewModel as ApplicationViewModel);
                return;
            }
            if (typeof(TViewModel) == typeof(ApplicationDialogModel))
            {
                SaveApplicationDialogModel(viewModel as ApplicationDialogModel);
                return;
            }
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }
        
        private static void SaveApplicationViewModel(ApplicationViewModel application)
        {
            var associatedApplicationFolder = ConfigurationService.Instance.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
            if (associatedApplicationFolder == null)
                throw new Exception(Strings.EntityNotFound);

            associatedApplicationFolder.Name = application.Name;
            associatedApplicationFolder.Path = application.Path;
            associatedApplicationFolder.EnableSilentInstallation = application.EnableSilentInstallation;
            associatedApplicationFolder.DisableReboot = application.DisableReboot;
            associatedApplicationFolder.EnableInstallationLogging = application.EnableInstallationLogging;

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

            installer.IsDirty = false;
        }

        private static void SaveApplicationDialogModel(ApplicationDialogModel applicationDialog)
        {
            ApplicationFolder applicationFolder;
            if (applicationDialog.FolderId == default(Guid))
            {
                applicationFolder = new ApplicationFolder
                {
                    Id = Guid.NewGuid()
                };
                ConfigurationService.Instance.Configuration.ApplicationFolders.Add(applicationFolder);
            } else
            {
                applicationFolder = ConfigurationService.Instance.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == applicationDialog.FolderId);
                if (applicationFolder == null)
                    throw new Exception(Strings.EntityNotFound);
            }
            
            applicationFolder.Name = applicationDialog.Name;
            applicationFolder.Path = applicationDialog.Path;
            applicationFolder.EnableSilentInstallation = applicationDialog.EnableSilentInstallation;
            applicationFolder.DisableReboot = applicationDialog.DisableReboot;
            applicationFolder.EnableInstallationLogging = applicationDialog.EnableInstallationLogging;

            applicationDialog.IsDirty = false;
        }

        public void UpdateViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                UpdateApplicationViewModel(viewModel as ApplicationViewModel);
                return;
            }
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }
        
        private static void UpdateApplicationViewModel(ApplicationViewModel application, ApplicationFolder applicationFolder = null)
        {
            if (application.FolderId == default(Guid))
                throw new Exception(Strings.EntityNotFound);

            if (applicationFolder == null)
                applicationFolder = ConfigurationService.Instance.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);

            if (applicationFolder == null)
                throw new Exception(Strings.ApplicationFolderNotCached);

            application.FolderId = applicationFolder.Id;
            application.Name = applicationFolder.Name;
            application.Path = applicationFolder.Path;
            application.EnableSilentInstallation = applicationFolder.EnableSilentInstallation;
            application.DisableReboot = applicationFolder.DisableReboot;
            application.EnableInstallationLogging = applicationFolder.EnableInstallationLogging;

            var installerBundles = CreateOrUpdateInstallerBundleViewModels(applicationFolder, application, application.InstallerBundles).ToList();
            application.InstallerBundles.Clear();
            foreach (var installerBundle in installerBundles)
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;
        }

        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(ApplicationFolder applicationFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var subFolder in applicationFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(SubFolder subFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingViewModels = null)
        {
            var existingInstallerBundles = existingViewModels?.ToList();
            foreach (var installerFileGroup in subFolder.FindAllInstallerFiles().GroupBy(i => i.Culture).Select(ifg => ifg.ToList()))
            {
                var currentCulture = installerFileGroup.FirstOrDefault()?.Culture;

                var installerBundleViewModel = existingInstallerBundles?.FirstOrDefault(ib => ib.Path == subFolder.Path && ib.Culture == currentCulture);
                if (installerBundleViewModel == null)
                {
                    installerBundleViewModel = new InstallerBundleViewModel
                    {
                        Parent = parent,
                        Name = subFolder.Name,
                        Path = subFolder.Path
                    };
                }

                // remove all InstallerViewModel's which don't exist in the installerFileGroup anymore
                var installerViewModelsToRemove = installerBundleViewModel.Installers.Where(i => installerFileGroup.All(installerFile => installerFile.Path != i.Path)).ToList();
                foreach (var installerViewModelToRemove in installerViewModelsToRemove)
                    installerBundleViewModel.Installers.Remove(installerViewModelToRemove);

                foreach (var installerFile in installerFileGroup)
                {
                    var installerViewModel = installerBundleViewModel.Installers.FirstOrDefault(i => i.Path == installerFile.Path);
                    if (installerViewModel == null)
                    {
                        installerViewModel = new InstallerViewModel
                        {
                            Parent = installerBundleViewModel,
                            PreferredOperation = InstallerOperationType.DoNothing
                        };
                        installerBundleViewModel.Installers.Add(installerViewModel);
                    }

                    installerViewModel.Name = installerFile.Name;
                    installerViewModel.Path = installerFile.Path;
                    installerViewModel.Version = installerFile.Version;
                    installerViewModel.Culture = installerFile.Culture;
                    installerViewModel.ProductCode = installerFile.ProductCode;
                    installerViewModel.IsInstalled = InstallService.Instance.IsProductCodeInstalled(installerFile.ProductCode);
                    installerViewModel.Created = installerFile.Created;
                    installerViewModel.PreferredOperation = InstallerOperationType.DoNothing;

                    installerViewModel.IsDirty = false;
                }

                installerBundleViewModel.IsDirty = false;

                yield return installerBundleViewModel;
            }

            foreach (var subSubFolder in subFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subSubFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        public void DeleteViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                DeleteApplicationViewModel(viewModel as ApplicationViewModel);
                return;
            }
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        private static void DeleteApplicationViewModel(ApplicationViewModel application)
        {
            ConfigurationService.Instance.Configuration.ApplicationFolders.RemoveAll(af => af.Id == application.FolderId);
        }
    }
}
