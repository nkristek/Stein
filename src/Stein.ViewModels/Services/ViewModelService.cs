using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.ViewModels;
using Stein.Localization;
using Stein.Presentation;
using Stein.Services.Configuration;
using Stein.Services.Configuration.v2;
using Stein.Services.InstallerFiles.Base;
using Stein.Services.InstallService;
using Stein.Services.MsiService;
using Stein.Services.ProductService;
using Stein.ViewModels.Commands.AboutDialogModelCommands;
using Stein.ViewModels.Commands.ApplicationDialogModelCommands;
using Stein.ViewModels.Commands.ApplicationViewModelCommands;
using Stein.ViewModels.Commands.DiskInstallerFileBundleProviderViewModelCommands;
using Stein.ViewModels.Commands.ExceptionViewModelCommands;
using Stein.ViewModels.Commands.InstallationViewModelCommands;
using Stein.ViewModels.Commands.MainWindowDialogModelCommands;
using Stein.ViewModels.Commands.UpdateDialogModelCommands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Services
{
    public sealed class ViewModelService
        : IViewModelService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IThemeService _themeService;

        private readonly IProgressBarService _progressBarService;

        private readonly IConfigurationService _configurationService;

        private readonly IInstallService _installService;

        private readonly IProductService _productService;
        
        private readonly IMsiService _msiService;

        private readonly IInstallerFileBundleProviderFactory _installerFileBundleProviderFactory;

        private readonly IUriService _uriService;

        private readonly string _tmpFolderPath;

        private readonly string _downloadFolderPath;

        public ViewModelService(IDialogService dialogService, IThemeService themeService, IProgressBarService progressBarService, IConfigurationService configurationService, IInstallService installService, IProductService productService, IMsiService msiService, IInstallerFileBundleProviderFactory installerFileBundleProviderFactory, IUriService uriService, string tmpFolderPath)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _progressBarService = progressBarService ?? throw new ArgumentNullException(nameof(progressBarService));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _msiService = msiService ?? throw new ArgumentNullException(nameof(msiService));
            _installerFileBundleProviderFactory = installerFileBundleProviderFactory ?? throw new ArgumentNullException(nameof(installerFileBundleProviderFactory));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            _tmpFolderPath = !String.IsNullOrEmpty(tmpFolderPath) ? tmpFolderPath : throw new ArgumentNullException(nameof(tmpFolderPath));
            _downloadFolderPath = GetDownloadFolderPath(tmpFolderPath);
        }

        private static string GetDownloadFolderPath(string parentFolderPath)
        {
            var downloadFolderPath = Path.Combine(parentFolderPath, "Downloads");
            if (!Directory.Exists(downloadFolderPath))
                Directory.CreateDirectory(downloadFolderPath);
            return downloadFolderPath;
        }

        /// <inheritdoc />
        public TViewModel CreateViewModel<TViewModel>(IViewModel parent = null, object entity = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;

            if (typeof(TViewModel) == typeof(MainWindowDialogModel))
                viewModel = CreateMainWindowDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(ApplicationDialogModel))
                viewModel = CreateApplicationDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(InstallerBundleDialogModel))
                viewModel = CreateInstallerBundleDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(AboutDialogModel))
                viewModel = CreateAboutDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(ExceptionViewModel))
                viewModel = CreateExceptionViewModel(parent, entity as Exception) as TViewModel;
            else if (typeof(TViewModel) == typeof(ExceptionDialogModel))
                viewModel = CreateExceptionDialogModel(parent, entity as Exception) as TViewModel;
            else if (typeof(TViewModel) == typeof(InstallationResultViewModel))
                viewModel = CreateInstallationResultViewModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(InstallationResultDialogModel))
                viewModel = CreateInstallationResultDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(InstallationViewModel))
                viewModel = CreateInstallationViewModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(UpdateDialogModel))
                viewModel = CreateUpdateDialogModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(UpdateAssetViewModel))
                viewModel = CreateUpdateAssetViewModel(parent) as TViewModel;
            else if (typeof(TViewModel) == typeof(WelcomeDialogModel))
                viewModel = CreateWelcomeDialogModel(parent) as TViewModel;
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            if (viewModel != null)
                viewModel.IsDirty = false;
            return viewModel;
        }

        private UpdateDialogModel CreateUpdateDialogModel(IViewModel parent)
        {
            var dialogModel = new UpdateDialogModel
            {
                Parent = parent,
                Title = Strings.UpdateAvailable,
                IsDirty = false
            };
            dialogModel.AddCommand(new OpenUpdateUriCommand(_uriService)
            {
                Parent = dialogModel
            });
            dialogModel.AddCommand(new InstallUpdateCommand(_installService, _downloadFolderPath)
            {
                Parent = dialogModel
            });
            dialogModel.AddCommand(new CancelUpdateCommand
            {
                Parent = dialogModel
            });
            return dialogModel;
        }

        private WelcomeDialogModel CreateWelcomeDialogModel(IViewModel parent)
        {
            return new WelcomeDialogModel
            {
                Parent = parent,
                IsDirty = false
            };
        }

        private UpdateAssetViewModel CreateUpdateAssetViewModel(IViewModel parent)
        {
            return new UpdateAssetViewModel
            {
                Parent = parent,
                IsDirty = false
            };
        }

        private InstallationViewModel CreateInstallationViewModel(IViewModel parent)
        {
            var viewModel = new InstallationViewModel(_progressBarService)
            {
                Parent = parent,
                IsDirty = false
            };
            viewModel.AddCommand(new CancelOperationCommand
            {
                Parent = viewModel
            });
            return viewModel;
        }

        private InstallationResultViewModel CreateInstallationResultViewModel(IViewModel parent)
        {
            if (!(parent is InstallerViewModel installer))
                throw new ArgumentException($"Argument has to be of type {nameof(InstallerViewModel)}", nameof(parent));

            return new InstallationResultViewModel
            {
                Parent = parent,
                InstallerName = installer.FileName,
                IsDirty = false
            };
        }

        private InstallationResultDialogModel CreateInstallationResultDialogModel(IViewModel parent)
        {
            if (!(parent is InstallationViewModel installationViewModel))
                throw new ArgumentException($"Argument has to be of type {nameof(InstallationViewModel)}", nameof(parent));

            var dialogModel = new InstallationResultDialogModel
            {
                Name = installationViewModel.Name,
                Parent = parent,
                IsDirty = false
            };
            dialogModel.AddCommand(new Commands.InstallationResultDialogModelCommands.OpenLogFolderCommand(_uriService)
            {
                Parent = dialogModel
            });
            return dialogModel;
        }

        private ExceptionViewModel CreateExceptionViewModel(IViewModel parent, Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            var exceptionViewModel = new ExceptionViewModel
            {
                Parent = parent,
                Message = exception.Message,
                TypeName = exception.GetType().Name,
                StackTrace = exception.StackTrace
            };

            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                    exceptionViewModel.InnerExceptions.Add(CreateExceptionViewModel(exceptionViewModel, innerException));
            }
            else if (exception.InnerException != null)
            {
                exceptionViewModel.InnerExceptions.Add(CreateExceptionViewModel(exceptionViewModel, exception.InnerException));
            }

            exceptionViewModel.AddCommand(new CopyExceptionDetailsToClipboardCommand
            {
                Parent = exceptionViewModel
            });

            exceptionViewModel.IsReadOnly = true;
            exceptionViewModel.IsDirty = false;
            return exceptionViewModel;
        }

        private ExceptionDialogModel CreateExceptionDialogModel(IViewModel parent, Exception exception)
        {
            var exceptionViewModel = parent as ExceptionViewModel;
            if (exceptionViewModel == null && exception == null)
                throw new ArgumentException($"Since no exception is provided via the entity parameter, parent has to be of type {nameof(ExceptionViewModel)}", nameof(parent));

            if (exceptionViewModel == null)
                exceptionViewModel = CreateExceptionViewModel(parent, exception);

            return new ExceptionDialogModel
            {
                Exception = exceptionViewModel,
                Parent = exceptionViewModel,
                IsReadOnly = true,
                IsDirty = false
            };
        }

        private MainWindowDialogModel CreateMainWindowDialogModel(IViewModel parent = null)
        {
            var viewModel = new MainWindowDialogModel(_themeService)
            {
                Parent = parent,
                Title = Assembly.GetEntryAssembly().GetName().Name
        };

            viewModel.AddCommand(new RefreshApplicationsCommand(this)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new AddApplicationCommand(_dialogService, this)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new ShowInfoDialogCommand(_dialogService, this)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new ChangeThemeCommand(this)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new ShowRecentInstallationResultCommand(_dialogService)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new ShowUpdateDialogCommand(_dialogService)
            {
                Parent = viewModel
            });

            foreach (var application in CreateApplicationViewModels(viewModel))
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
            return viewModel;
        }

        private IEnumerable<ApplicationViewModel> CreateApplicationViewModels(IViewModel parent)
        {
            foreach (var application in _configurationService.Configuration.Applications)
                yield return CreateApplicationViewModel(application, parent);
        }

        private ApplicationViewModel CreateApplicationViewModel(Application application, IViewModel parent = null)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            var viewModel = new ApplicationViewModel
            {
                Parent = parent,
                EntityId = application.Id,
                Name = application.Name,
                EnableSilentInstallation = application.EnableSilentInstallation,
                DisableReboot = application.DisableReboot,
                EnableInstallationLogging = application.EnableInstallationLogging,
                AutomaticallyDeleteInstallationLogs = application.AutomaticallyDeleteInstallationLogs,
                KeepNewestInstallationLogs = application.KeepNewestInstallationLogs,
                FilterDuplicateInstallers = application.FilterDuplicateInstallers,
                ProviderType = application.Configuration?.ProviderType
            };
            viewModel.AddCommand(new EditApplicationCommand(_dialogService, this)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new DeleteApplicationCommand(this)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new InstallApplicationCommand(_dialogService, this, _installService, _downloadFolderPath)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new UninstallApplicationCommand(_dialogService, this, _installService, _downloadFolderPath)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new CustomOperationApplicationCommand(_dialogService, this, _installService, _downloadFolderPath)
            {
                Parent = viewModel
            });
            viewModel.AddCommand(new OpenProviderLinkCommand(_uriService)
            {
                Parent = viewModel
            });

            viewModel.IsDirty = false;
            return viewModel;
        }

        private Guid GetNewApplicationId()
        {
            var id = Guid.NewGuid();
            while (_configurationService.Configuration.Applications.Any(a => id.Equals(a.Id)))
                id = Guid.NewGuid();
            return id;
        }
        
        private ApplicationDialogModel CreateApplicationDialogModel(IViewModel parent = null)
        {
            ApplicationDialogModel dialogModel;
            if (parent is ApplicationViewModel applicationViewModel)
            {
                dialogModel = new ApplicationDialogModel
                {
                    Parent = applicationViewModel,
                    Title = Strings.Edit,
                    EntityId = applicationViewModel.EntityId,
                    Name = applicationViewModel.Name,
                    EnableSilentInstallation = applicationViewModel.EnableSilentInstallation,
                    DisableReboot = applicationViewModel.DisableReboot,
                    EnableInstallationLogging = applicationViewModel.EnableInstallationLogging,
                    AutomaticallyDeleteInstallationLogs = applicationViewModel.AutomaticallyDeleteInstallationLogs,
                    KeepNewestInstallationLogs = applicationViewModel.KeepNewestInstallationLogs,
                    FilterDuplicateInstallers = applicationViewModel.FilterDuplicateInstallers
                };

                foreach (var provider in CreateAvailableProviderViewModels(dialogModel))
                    dialogModel.AvailableProviders.Add(provider);

                var application = _configurationService.Configuration.Applications.FirstOrDefault(af => af.Id == applicationViewModel.EntityId);
                if (application == null)
                    throw new InvalidOperationException(Strings.EntityNotFound);

                var matchingProvider = dialogModel.AvailableProviders.FirstOrDefault(p => p.ProviderType.ToString() == application.Configuration.ProviderType);
                if (matchingProvider == null)
                    throw new InvalidOperationException("The installer file provider is not supported.");

                matchingProvider.LoadConfiguration(application.Configuration);
                dialogModel.SelectedProvider = matchingProvider;
            }
            else
            {
                var defaultValues = new Application();
                dialogModel = new ApplicationDialogModel
                {
                    Parent = parent,
                    Title = Strings.Add,
                    Name = String.Empty,
                    EnableSilentInstallation = defaultValues.EnableSilentInstallation,
                    DisableReboot = defaultValues.DisableReboot,
                    EnableInstallationLogging = defaultValues.EnableInstallationLogging,
                    AutomaticallyDeleteInstallationLogs = defaultValues.AutomaticallyDeleteInstallationLogs,
                    KeepNewestInstallationLogs = defaultValues.KeepNewestInstallationLogs,
                    FilterDuplicateInstallers = defaultValues.FilterDuplicateInstallers
                };
                foreach (var provider in CreateAvailableProviderViewModels(dialogModel))
                    dialogModel.AvailableProviders.Add(provider);
                dialogModel.SelectedProvider = dialogModel.AvailableProviders.FirstOrDefault();
            }
            
            dialogModel.AddCommand(new OpenLogFolderCommand(_uriService)
            {
                Parent = dialogModel
            });

            dialogModel.IsDirty = false;
            return dialogModel;
        }
        
        private IEnumerable<InstallerFileBundleProviderViewModel> CreateAvailableProviderViewModels(IViewModel parent = null)
        {
            var diskProvider = new DiskInstallerFileBundleProviderViewModel
            {
                Parent = parent,
                IsDirty = false
            };
            diskProvider.AddCommand(new SelectFolderCommand
            {
                Parent = diskProvider
            });
            yield return diskProvider;

            yield return new GitHubInstallerFileBundleProviderViewModel
            {
                Parent = parent,
                IsDirty = false
            };
        }

        private InstallerBundleDialogModel CreateInstallerBundleDialogModel(IViewModel parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            if (!(parent is InstallerBundleViewModel installerBundle))
                throw new ArgumentException($"Only parents of type {nameof(InstallerBundleViewModel)} are accepted. (got: {parent.GetType().Name}", nameof(parent));

            return new InstallerBundleDialogModel
            {
                Parent = installerBundle,
                Title = installerBundle.Name,
                Name = installerBundle.Name,
                IsDirty = false
            };
        }

        private AboutDialogModel CreateAboutDialogModel(IViewModel parent = null)
        {
            var viewModel = new AboutDialogModel
            {
                Parent = parent
            };

            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Smaragd",
                Uri = new Uri("https://github.com/nkristek/Smaragd"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Wpf.Converters",
                Uri = new Uri("https://github.com/nkristek/Wpf.Converters"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "AdonisUI",
                Uri = new Uri("https://github.com/benruehl/adonis-ui"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "log4net",
                Uri = new Uri("http://logging.apache.org/log4net/"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Newtonsoft.Json",
                Uri = new Uri("https://www.newtonsoft.com/json"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Ninject",
                Uri = new Uri("https://github.com/ninject/Ninject"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Windows-API-Code-Pack",
                Uri = new Uri("https://github.com/aybe/Windows-API-Code-Pack-1.1"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "Wix Toolset",
                Uri = new Uri("http://wixtoolset.org/"),
                IsDirty = false
            });
            viewModel.Dependencies.Add(new DependencyViewModel
            {
                Name = "System.Windows.Interactivity.WPF",
                Uri = new Uri("https://www.microsoft.com/en-us/download/details.aspx?id=10801"),
                IsDirty = false
            });

            viewModel.AddCommand(new OpenUriCommand(_uriService)
            {
                Parent = viewModel
            });

            foreach (var dependency in viewModel.Dependencies)
            { 
                dependency.AddCommand(new Commands.DependencyViewModelCommands.OpenUriCommand(_uriService)
                {
                    Parent = dependency
                });
                dependency.IsDirty = false;
            }

            viewModel.IsReadOnly = true;
            viewModel.IsDirty = false;
            return viewModel;
        }
        
        /// <inheritdoc />
        public async Task SaveViewModelAsync<TViewModel>(TViewModel viewModel, object entity = null) where TViewModel : class, IViewModel
        {
            if (!viewModel.IsDirty)
                return;

            if (typeof(TViewModel) == typeof(MainWindowDialogModel))
                SaveMainWindowDialogModel(viewModel as MainWindowDialogModel);
            else if (typeof(TViewModel) == typeof(ApplicationDialogModel))
                SaveApplicationDialogModel(viewModel as ApplicationDialogModel);
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            await _configurationService.SaveConfigurationAsync();
            viewModel.IsDirty = false;
        }

        private void SaveMainWindowDialogModel(MainWindowDialogModel viewModel)
        {
            _configurationService.Configuration.SelectedTheme = viewModel.CurrentTheme;
            viewModel.IsDirty = false;
        }
        
        private void SaveApplicationDialogModel(ApplicationDialogModel applicationDialog)
        {
            if (applicationDialog.SelectedProvider == null)
                throw new ArgumentException("The selected provider is null", nameof(applicationDialog));

            Application applicationFolder;
            if (applicationDialog.EntityId == default)
            {
                applicationDialog.EntityId = GetNewApplicationId();
                applicationFolder = new Application
                {
                    Id = applicationDialog.EntityId
                };
                _configurationService.Configuration.Applications.Add(applicationFolder);
            }
            else
            {
                applicationFolder = _configurationService.Configuration.Applications.FirstOrDefault(af => af.Id == applicationDialog.EntityId);
                if (applicationFolder == null)
                    throw new Exception(Strings.EntityNotFound);
            }
            
            applicationFolder.Name = applicationDialog.Name;
            applicationFolder.EnableSilentInstallation = applicationDialog.EnableSilentInstallation;
            applicationFolder.DisableReboot = applicationDialog.DisableReboot;
            applicationFolder.EnableInstallationLogging = applicationDialog.EnableInstallationLogging;
            applicationFolder.AutomaticallyDeleteInstallationLogs = applicationDialog.AutomaticallyDeleteInstallationLogs;
            applicationFolder.KeepNewestInstallationLogs = applicationDialog.KeepNewestInstallationLogs;
            applicationFolder.Configuration = applicationDialog.SelectedProvider.CreateConfiguration();
            applicationFolder.FilterDuplicateInstallers = applicationDialog.FilterDuplicateInstallers;

            if (applicationDialog.Parent is MainWindowDialogModel mainWindowDialogModel)
            {
                var applicationViewModel = CreateApplicationViewModel(applicationFolder, mainWindowDialogModel);
                mainWindowDialogModel.Applications.Add(applicationViewModel);
            }

            applicationDialog.IsDirty = false;
        }

        /// <inheritdoc />
        public async Task UpdateViewModelAsync<TViewModel>(TViewModel viewModel, object entity = null) where TViewModel : class, IViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                await UpdateApplicationViewModelAsync(viewModel as ApplicationViewModel);
            else if (typeof(TViewModel) == typeof(MainWindowDialogModel))
                await UpdateMainWindowDialogModelAsync(viewModel as MainWindowDialogModel);
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            viewModel.IsDirty = false;
        }

        private async Task UpdateMainWindowDialogModelAsync(MainWindowDialogModel viewModel)
        {
            viewModel.IsUpdating = true;

            // TODO: modifying/replacing the ObservableCollection freezes WPF for a short time

            //var applications = new ObservableCollection<ApplicationViewModel>(CreateApplicationViewModels(viewModel));
            //viewModel.Applications = applications;

            //viewModel.Applications.Clear();
            //foreach (var applicationViewModel in CreateApplicationViewModels(viewModel))
            //    viewModel.Applications.Add(applicationViewModel);

            foreach (var applicationViewModel in viewModel.Applications)
                applicationViewModel.IsUpdating = true;

            try
            {
                await Task.Run(() => _productService.RefreshInstalledProducts());
            }
            catch (Exception exception)
            {
                Log.Error("Getting installed products failed", exception);
            }

            var updateTasks = viewModel.Applications.Select(async application => await UpdateApplicationViewModelAsync(application));
            await Task.WhenAll(updateTasks);

            viewModel.IsUpdating = false;
            viewModel.IsDirty = false;
        }

        private async Task UpdateApplicationViewModelAsync(ApplicationViewModel viewModel, Application application = null)
        {
            if (application == null)
            {
                if (viewModel.EntityId == default)
                    throw new Exception(Strings.EntityNotFound);

                application = _configurationService.Configuration.Applications.FirstOrDefault(af => af.Id == viewModel.EntityId);
            }

            if (application == null)
                throw new Exception(Strings.EntityNotFound);

            viewModel.EntityId = application.Id;
            viewModel.Name = application.Name;
            viewModel.EnableSilentInstallation = application.EnableSilentInstallation;
            viewModel.DisableReboot = application.DisableReboot;
            viewModel.EnableInstallationLogging = application.EnableInstallationLogging;
            viewModel.AutomaticallyDeleteInstallationLogs = application.AutomaticallyDeleteInstallationLogs;
            viewModel.KeepNewestInstallationLogs = application.KeepNewestInstallationLogs;
            viewModel.FilterDuplicateInstallers = application.FilterDuplicateInstallers;
            viewModel.ProviderType = application.Configuration?.ProviderType;
            
            viewModel.InstallerBundles.Clear();
            try
            {
                var configuration = application.Configuration;
                if (configuration == null)
                    throw new Exception("No configuration is set");

                using (var provider = _installerFileBundleProviderFactory.Create(configuration))
                {
                    viewModel.ProviderLink = provider.ProviderLink;

                    var installerBundles = await provider.GetInstallerFileBundlesAsync();
                    foreach (var installerBundle in installerBundles)
                    {
                        var bundleViewModel = new InstallerBundleViewModel
                        {
                            Parent = viewModel,
                            Name = installerBundle.Name,
                            Created = installerBundle.Created,
                        };
                        foreach (var installerFile in installerBundle.InstallerFiles)
                        {
                            var installerViewModel = new InstallerViewModel
                            {
                                Parent = bundleViewModel,
                                FileName = installerFile.FileName,
                                Created = installerFile.Created
                            };
                            installerViewModel.InstallerFileProvider = new InstallerFileProvider(async (filePath, progress, cancellationToken) =>
                            {
                                await installerFile.SaveFileAsync(filePath, _msiService, progress, cancellationToken);
                                installerViewModel.Name = installerFile.Name;
                                installerViewModel.Culture = installerFile.Culture.IetfLanguageTag;
                                installerViewModel.Version = installerFile.Version;
                                installerViewModel.ProductCode = installerFile.ProductCode;
                                installerViewModel.IsInstalled = _productService.IsProductInstalled(installerFile.ProductCode);
                            });
                            installerViewModel.IsDirty = false;

                            bundleViewModel.Installers.Add(installerViewModel);
                        }
                        viewModel.InstallerBundles.Add(bundleViewModel);
                        bundleViewModel.IsDirty = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error("Getting installer file bundles failed", exception);
                viewModel.InstallerBundles.Clear();
            }
            
            viewModel.SelectedInstallerBundle = viewModel.InstallerBundles.LastOrDefault();

            viewModel.IsUpdating = false;
            viewModel.IsDirty = false;
        }
        
        /// <inheritdoc />
        public async Task DeleteViewModelAsync<TViewModel>(TViewModel viewModel, object entity = null) where TViewModel : class, IViewModel
        {
            if (typeof(TViewModel) == typeof(ApplicationViewModel))
                DeleteApplicationViewModel(viewModel as ApplicationViewModel);
            else
                throw new NotSupportedException(Strings.ViewModelNotSupported);

            await _configurationService.SaveConfigurationAsync();
        }

        private void DeleteApplicationViewModel(ApplicationViewModel application)
        {
            _configurationService.Configuration.Applications.RemoveAll(af => af.Id == application.EntityId);
        }
    }
}
