using System;
using System.Collections.Generic;
using System.Linq;
using NKristek.Smaragd.ViewModels;
using Stein.Localizations;
using Stein.Presentation;
using Stein.Services;
using Stein.Services.Extensions;
using Stein.Services.Types;
using Stein.ViewModels.Commands.ApplicationDialogModelCommands;
using Stein.ViewModels.Commands.ApplicationViewModelCommands;
using Stein.ViewModels.Commands.MainWindowViewModelCommands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class ViewModelService
        : IViewModelService
    {
        private readonly IDialogService _dialogService;

        private readonly IThemeService _themeService;

        private readonly IProgressBarService _progressBarService;

        private readonly IConfigurationService _configurationService;

        private readonly IInstallService _installService;
        
        private readonly IMsiService _msiService;

        public ViewModelService(IDialogService dialogService, IThemeService themeService, IProgressBarService progressBarService, IConfigurationService configurationService, IInstallService installService, IMsiService msiService)
        {
            _dialogService = dialogService;
            _themeService = themeService;
            _progressBarService = progressBarService;
            _configurationService = configurationService;
            _installService = installService;
            _msiService = msiService;
        }

        public TViewModel CreateViewModel<TViewModel>(ViewModel parent = null) where TViewModel : ViewModel
        {
            TViewModel viewModel;

            if (typeof(TViewModel) == typeof(MainWindowViewModel))
                viewModel = CreateMainWindowViewModel() as TViewModel;
            else if (typeof(TViewModel) == typeof(ApplicationViewModel))
                viewModel = CreateApplicationViewModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(ApplicationDialogModel))
                viewModel = CreateApplicationDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(InstallerBundleDialogModel))
                viewModel = CreateInstallerBundleDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(AboutDialogModel))
                viewModel = CreateAboutDialogModel(parent) as TViewModel;
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            if (viewModel != null)
                viewModel.IsDirty = false;
            return viewModel;
        }

        private MainWindowViewModel CreateMainWindowViewModel()
        {
            var viewModel = new MainWindowViewModel(_themeService, _progressBarService);
            viewModel.RefreshApplicationsCommand = new RefreshApplicationsCommand(viewModel, _dialogService, this, _configurationService, _installService, _msiService);
            viewModel.AddApplicationCommand = new AddApplicationCommand(viewModel, _dialogService, this);
            viewModel.CancelOperationCommand = new CancelOperationCommand(viewModel, _dialogService);
            viewModel.ShowInfoDialogCommand = new ShowInfoDialogCommand(viewModel, _dialogService, this);
            viewModel.ChangeThemeCommand = new ChangeThemeCommand(viewModel, _dialogService);

            foreach (var application in CreateApplicationViewModels(viewModel, Enumerable.Empty<ApplicationViewModel>()))
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
            return viewModel;
        }

        private ApplicationViewModel CreateApplicationViewModel(ViewModel parent, ApplicationFolder applicationFolder = null)
        {
            if (applicationFolder == null)
            {
                applicationFolder = new ApplicationFolder
                {
                    Id = Guid.NewGuid()
                };
                _configurationService.Configuration.ApplicationFolders.Add(applicationFolder);
                _configurationService.SaveConfiguration();
            }

            var viewModel = new ApplicationViewModel
            {
                Parent = parent,
                FolderId = applicationFolder.Id,
                Name = applicationFolder.Name,
                Path = applicationFolder.Path,
                EnableSilentInstallation = applicationFolder.EnableSilentInstallation,
                DisableReboot = applicationFolder.DisableReboot,
                EnableInstallationLogging = applicationFolder.EnableInstallationLogging
            };
            viewModel.EditApplicationCommand = new EditApplicationCommand(viewModel, _dialogService, this);
            viewModel.DeleteApplicationCommand = new DeleteApplicationCommand(viewModel, _dialogService, this);
            viewModel.InstallApplicationCommand = new InstallApplicationCommand(viewModel, _dialogService, _installService);
            viewModel.UninstallApplicationCommand = new UninstallApplicationCommand(viewModel, _dialogService, _installService);
            viewModel.CustomOperationApplicationCommand = new CustomOperationApplicationCommand(viewModel, _dialogService, _installService, this);
            
            var installerBundles = CreateOrUpdateInstallerBundleViewModels(applicationFolder, viewModel).ToList();
            viewModel.InstallerBundles.Clear();
            foreach (var installerBundle in installerBundles)
                viewModel.InstallerBundles.Add(installerBundle);
            viewModel.SelectedInstallerBundle = viewModel.InstallerBundles.LastOrDefault();

            viewModel.IsDirty = false;
            return viewModel;
        }

        private ApplicationDialogModel CreateApplicationDialogModel(ViewModel parent)
        {
            ApplicationDialogModel viewModel;
            if (parent is ApplicationViewModel application)
            {
                viewModel = new ApplicationDialogModel
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
            else
            {
                viewModel = new ApplicationDialogModel
                {
                    Parent = parent,
                    Title = Strings.AddFolder,
                    Name = String.Empty,
                    Path = String.Empty,
                    EnableSilentInstallation = true,
                    DisableReboot = true,
                    EnableInstallationLogging = true
                };
            }

            viewModel.SelectFolderCommand = new SelectFolderCommand(viewModel, _dialogService);
            viewModel.OpenLogFolderCommand = new OpenLogFolderCommand(viewModel, _dialogService);

            viewModel.IsDirty = false;
            return viewModel;
        }

        private InstallerBundleDialogModel CreateInstallerBundleDialogModel(ViewModel parent)
        {
            if (!(parent is InstallerBundleViewModel installerBundle))
                throw new ArgumentNullException(nameof(parent));

            return new InstallerBundleDialogModel
            {
                Parent = installerBundle,
                Title = installerBundle.Name,
                Name = installerBundle.Name,
                Path = installerBundle.Path,
                IsDirty = false
            };
        }

        private static AboutDialogModel CreateAboutDialogModel(ViewModel parent)
        {
            var viewModel = new AboutDialogModel
            {
                Parent = parent
            };
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Smaragd",
                Uri = new Uri("https://github.com/nkristek/Smaragd")
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Wpf.Converters",
                Uri = new Uri("https://github.com/nkristek/Wpf.Converters")
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "GongSolutions.WPF.DragDrop",
                Uri = new Uri("https://github.com/punker76/gong-wpf-dragdrop")
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "log4net",
                Uri = new Uri("http://logging.apache.org/log4net/")
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Windows-API-Code-Pack",
                Uri = new Uri("https://github.com/aybe/Windows-API-Code-Pack-1.1")
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Wix Toolset",
                Uri = new Uri("http://wixtoolset.org/")
            });
            viewModel.IsDirty = false;
            return viewModel;
        }

        public IEnumerable<TViewModel> CreateViewModels<TViewModel>(ViewModel parent = null, IEnumerable<TViewModel> existingViewModels = null) where TViewModel : ViewModel
        {
            IEnumerable<TViewModel> viewModels;
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                viewModels = CreateApplicationViewModels(parent, existingViewModels as IEnumerable<ApplicationViewModel>) as IEnumerable<TViewModel>;
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            var viewModelsList = viewModels?.ToList() ?? new List<TViewModel>();
            foreach (var viewModel in viewModelsList)
                viewModel.IsDirty = false;
            return viewModelsList;
        }

        private IEnumerable<ApplicationViewModel> CreateApplicationViewModels(ViewModel parent, IEnumerable<ApplicationViewModel> existingViewModels)
        {
            var existingApplicationViewModels = existingViewModels.ToList();
            foreach (var applicationFolder in _configurationService.Configuration.ApplicationFolders)
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

            if (typeof(TViewModel) == typeof(MainWindowViewModel))
                SaveMainWindowViewModel(viewModel as MainWindowViewModel);
            else if (typeof(TViewModel) == typeof(ApplicationViewModel))
                SaveApplicationViewModel(viewModel as ApplicationViewModel);
            else if (typeof(TViewModel) == typeof(ApplicationDialogModel))
                SaveApplicationDialogModel(viewModel as ApplicationDialogModel);
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            viewModel.IsDirty = false;
        }

        private void SaveMainWindowViewModel(MainWindowViewModel viewModel)
        {
            foreach (var application in viewModel.Applications.Where(a => a.IsDirty))
                SaveApplicationViewModel(application);

            viewModel.IsDirty = false;
        }
        
        private void SaveApplicationViewModel(ApplicationViewModel application)
        {
            var applicationFolder = _configurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
            if (applicationFolder == null)
                throw new Exception(Strings.EntityNotFound);

            applicationFolder.Name = application.Name;
            applicationFolder.Path = application.Path;
            applicationFolder.EnableSilentInstallation = application.EnableSilentInstallation;
            applicationFolder.DisableReboot = application.DisableReboot;
            applicationFolder.EnableInstallationLogging = application.EnableInstallationLogging;
            
            _configurationService.SaveConfiguration();

            application.IsDirty = false;
        }
        
        private void SaveApplicationDialogModel(ApplicationDialogModel applicationDialog)
        {
            ApplicationFolder applicationFolder;
            if (applicationDialog.FolderId == default(Guid))
            {
                applicationFolder = new ApplicationFolder
                {
                    Id = Guid.NewGuid()
                };
                _configurationService.Configuration.ApplicationFolders.Add(applicationFolder);
            } else
            {
                applicationFolder = _configurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == applicationDialog.FolderId);
                if (applicationFolder == null)
                    throw new Exception(Strings.EntityNotFound);
            }
            
            applicationFolder.Name = applicationDialog.Name;
            applicationFolder.Path = applicationDialog.Path;
            applicationFolder.EnableSilentInstallation = applicationDialog.EnableSilentInstallation;
            applicationFolder.DisableReboot = applicationDialog.DisableReboot;
            applicationFolder.EnableInstallationLogging = applicationDialog.EnableInstallationLogging;

            _configurationService.SaveConfiguration();
        }

        public void UpdateViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                UpdateApplicationViewModel(viewModel as ApplicationViewModel);
            else if (typeof(TViewModel) == typeof(MainWindowViewModel))
                UpdateMainWindowViewModel(viewModel as MainWindowViewModel);
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            viewModel.IsDirty = false;
        }

        private void UpdateMainWindowViewModel(MainWindowViewModel viewModel)
        {
            var applications = CreateViewModels(viewModel, viewModel.Applications.ToList());
            viewModel.Applications.Clear();
            foreach (var application in applications)
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
        }

        private void UpdateApplicationViewModel(ApplicationViewModel application, ApplicationFolder applicationFolder = null)
        {
            if (application.FolderId == default(Guid))
                throw new Exception(Strings.EntityNotFound);

            if (applicationFolder == null)
                applicationFolder = _configurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);

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

        private IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(ApplicationFolder applicationFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingInstallerBundles = null)
        {
            foreach (var subFolder in applicationFolder.SubFolders)
                foreach (var installerBundle in CreateOrUpdateInstallerBundleViewModels(subFolder, parent, existingInstallerBundles))
                    yield return installerBundle;
        }

        private IEnumerable<InstallerBundleViewModel> CreateOrUpdateInstallerBundleViewModels(SubFolder subFolder, ViewModel parent = null, IEnumerable<InstallerBundleViewModel> existingViewModels = null)
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
                            PreferredOperation = InstallerOperation.DoNothing
                        };
                        installerBundleViewModel.Installers.Add(installerViewModel);
                    }

                    installerViewModel.Name = installerFile.Name;
                    installerViewModel.Path = installerFile.Path;
                    installerViewModel.Version = installerFile.Version;
                    installerViewModel.Culture = installerFile.Culture;
                    installerViewModel.ProductCode = installerFile.ProductCode;
                    installerViewModel.IsInstalled = _installService.IsProductCodeInstalled(installerFile.ProductCode);
                    installerViewModel.Created = installerFile.Created;
                    installerViewModel.PreferredOperation = InstallerOperation.DoNothing;
                    installerViewModel.IsDirty = false;
                }

                installerBundleViewModel.IsDirty = false;
                yield return installerBundleViewModel;
            }
        }

        public void DeleteViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                DeleteApplicationViewModel(viewModel as ApplicationViewModel);
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);
        }

        private void DeleteApplicationViewModel(ApplicationViewModel application)
        {
            _configurationService.Configuration.ApplicationFolders.RemoveAll(af => af.Id == application.FolderId);
            _configurationService.SaveConfiguration();
        }
    }
}
