using System;
using System.Collections.Generic;
using System.Linq;
using Stein.ConfigurationTypes;
using Stein.ViewModels;
using Stein.Localizations;
using nkristek.MVVMBase.ViewModels;

namespace Stein.Services
{
    public static class ViewModelService
    {
        /// <summary>
        /// Creates or updates existing applicationViewModels from the configuration
        /// </summary>
        /// <param name="parent">Parent of these ApplicationViewModels</param>
        /// <param name="existingApplicationViewModels">List of existing ApplicationViewModels</param>
        /// <returns>List of new or updated ApplicationViewModels</returns>
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

        /// <summary>
        /// Saves a ViewModel to the persistent entity
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of ViewModel</typeparam>
        /// <param name="viewModelToSave">ViewModel to save</param>
        public static void SaveViewModel<TViewModel>(TViewModel viewModelToSave) where TViewModel : ViewModel
        {
            if (!viewModelToSave.IsDirty)
                return;

            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                SaveApplicationViewModel(viewModelToSave as ApplicationViewModel);
                return;
            }
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        /// <summary>
        /// Saves an ApplicationViewModel to the corresponding ApplicationFolder in the configuration
        /// </summary>
        /// <param name="application">ApplicationViewModel to save</param>
        private static void SaveApplicationViewModel(ApplicationViewModel application)
        {
            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
            if (associatedApplicationFolder == null)
                return;

            associatedApplicationFolder.Name = application.Name;
            associatedApplicationFolder.Path = application.Path;
            associatedApplicationFolder.EnableSilentInstallation = application.EnableSilentInstallation;
            associatedApplicationFolder.EnableInstallationLogging = application.EnableInstallationLogging;

            foreach (var installerBundle in application.InstallerBundles.Where(installerBundle => installerBundle.IsDirty))
                SaveInstallerBundleViewModel(installerBundle, associatedApplicationFolder);

            application.IsDirty = false;
        }

        /// <summary>
        /// Saves an InstallerBundleViewModel to the corresponding SubFolder in the configuration
        /// </summary>
        /// <param name="installerBundle">InstallerBundleViewModel to save</param>
        /// <param name="parentApplicationFolder">The parent ApplicationFolder the SubFolder exists in</param>
        private static void SaveInstallerBundleViewModel(InstallerBundleViewModel installerBundle, ApplicationFolder parentApplicationFolder)
        {
            var associatedSubFolder = parentApplicationFolder.FindSubFolder(installerBundle.Path);
            if (associatedSubFolder == null)
                return;

            foreach (var installer in installerBundle.Installers.Where(installer => installer.IsDirty))
                SaveInstallerViewModel(installer, associatedSubFolder);
            
            installerBundle.IsDirty = false;
        }

        /// <summary>
        /// Saves an InstallerViewModel to the corresponding InstallerFile in the configuration
        /// </summary>
        /// <param name="installer">InstallerViewModel to save</param>
        /// <param name="parentSubFolder">The parent SubFolder the InstallerFile exists in</param>
        private static void SaveInstallerViewModel(InstallerViewModel installer, SubFolder parentSubFolder)
        {
            var installerFile = parentSubFolder.FindInstallerFile(installer.Path);
            if (installerFile == null)
                return;

            installer.IsDirty = false;
        }
        
        /// <summary>
        /// Create a ViewModel from the corresponding entity
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of ViewModel</typeparam>
        /// <param name="entity">Entity of the ViewModel</param>
        /// <param name="parent">Parent of the ViewModel (optional)</param>
        /// <returns>The created ViewModel</returns>
        public static TViewModel CreateViewModel<TViewModel>(object entity, ViewModel parent = null) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                return CreateApplicationViewModel(entity as ApplicationFolder, parent) as TViewModel;
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        /// <summary>
        /// Create an ApplicationViewModel from an ApplicationFolder
        /// </summary>
        /// <param name="applicationFolder">ApplicationFolder from the configuration</param>
        /// <param name="parent">Parent of the ApplicationViewModel (optional)</param>
        /// <returns>The created ApplicationViewModel</returns>
        private static ApplicationViewModel CreateApplicationViewModel(ApplicationFolder applicationFolder, ViewModel parent = null)
        {
            if (applicationFolder == null)
                throw new ArgumentNullException("applicationFolder");

            var application = new ApplicationViewModel(parent)
            {
                FolderId = applicationFolder.Id,
                Name = applicationFolder.Name,
                Path = applicationFolder.Path,
                EnableSilentInstallation = applicationFolder.EnableSilentInstallation,
                EnableInstallationLogging = applicationFolder.EnableInstallationLogging
            };

            foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(applicationFolder, application))
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;

            return application;
        }

        /// <summary>
        /// Create a List of InstallerBundleViewModel from a ApplicationFolder
        /// </summary>
        /// <param name="applicationFolder">ApplicationFolder from the configuration</param>
        /// <param name="parent">Parent of the InstallerBundleViewModels</param>
        /// <param name="existingInstallerBundles">List of existing InstallerBundleViewModels</param>
        /// <returns>List of new or updated InstallerBundleViewModels</returns>
        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(ApplicationFolder applicationFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var subFolder in applicationFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        /// <summary>
        /// Create a List of InstallerBundleViewModel from a SubFolder
        /// </summary>
        /// <param name="subFolder">SubFolder from the configuration</param>
        /// <param name="parent">Parent of the InstallerBundleViewModels</param>
        /// <param name="existingInstallerBundles">List of existing InstallerBundleViewModels</param>
        /// <returns>List of new or updated InstallerBundleViewModels</returns>
        private static IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(SubFolder subFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var installerFileGroup in subFolder.FindAllInstallerFiles().GroupBy(i => i.Culture).Select(ifg => ifg.ToList()))
            {
                var currentCulture = installerFileGroup.FirstOrDefault()?.Culture;

                var installerBundleViewModel = existingInstallerBundles?.FirstOrDefault(ib => ib.Path == subFolder.Path && ib.Culture == currentCulture);
                if (installerBundleViewModel == null)
                {
                    installerBundleViewModel = new InstallerBundleViewModel(parent)
                    {
                        Name = subFolder.Name,
                        Path = subFolder.Path
                    };
                }

                // remove all InstallerViewModel's which don't exist in the installerFileGroup anymore
                var installerViewModelsToRemove = installerBundleViewModel.Installers.Where(i => !installerFileGroup.Any(installerFile => installerFile.Path == i.Path)).ToList();
                foreach (var installerViewModelToRemove in installerViewModelsToRemove)
                    installerBundleViewModel.Installers.Remove(installerViewModelToRemove);
                
                foreach (var installerFile in installerFileGroup)
                {
                    var installerViewModel = installerBundleViewModel.Installers.FirstOrDefault(i => i.Path == installerFile.Path);
                    if (installerViewModel == null)
                    {
                        installerViewModel = new InstallerViewModel(installerBundleViewModel)
                        {
                            PreferredOperation = InstallerOperationType.DoNothing
                        };
                        installerBundleViewModel.Installers.Add(installerViewModel);
                    }

                    installerViewModel.Name = installerFile.Name;
                    installerViewModel.Path = installerFile.Path;
                    installerViewModel.Version = installerFile.Version;
                    installerViewModel.Culture = installerFile.Culture;
                    installerViewModel.ProductCode = installerFile.ProductCode;
                    installerViewModel.IsInstalled = InstallService.IsProductCodeInstalled(installerFile.ProductCode);
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

        /// <summary>
        /// Updates the given ViewModel and discard any changes made to it
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of ViewModel</typeparam>
        /// <param name="viewModelToUpdate">ViewModel to update</param>
        /// <param name="entity">Entity of the ViewModel</param>
        public static void UpdateViewModel<TViewModel>(TViewModel viewModelToUpdate, object entity = null) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
            {
                UpdateApplicationViewModel(viewModelToUpdate as ApplicationViewModel, entity as ApplicationFolder);
                return;
            }
            throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        /// <summary>
        /// Updates the given ApplicationViewModel and discard any changes made to it
        /// </summary>
        /// <param name="application">ApplicationViewModel to update</param>
        /// <param name="associatedApplicationFolder">ApplicationFolder from the configuration</param>
        private static void UpdateApplicationViewModel(ApplicationViewModel application, ApplicationFolder associatedApplicationFolder = null)
        {
            if (associatedApplicationFolder == null)
            {
                associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
                if (associatedApplicationFolder == null)
                    throw new Exception(Strings.ApplicationFolderNotCached);
            }

            application.FolderId = associatedApplicationFolder.Id;
            application.Name = associatedApplicationFolder.Name;
            application.Path = associatedApplicationFolder.Path;
            application.EnableSilentInstallation = associatedApplicationFolder.EnableSilentInstallation;
            application.EnableInstallationLogging = associatedApplicationFolder.EnableInstallationLogging;

            var installerBundles = CreateOrUpdateInstallerBundleViewModels(associatedApplicationFolder, application, application.InstallerBundles).ToList();
            application.InstallerBundles.Clear();
            foreach (var installerBundle in installerBundles)
                application.InstallerBundles.Add(installerBundle);

            application.SelectedInstallerBundle = application.InstallerBundles.LastOrDefault();

            application.IsDirty = false;
        }
    }
}
