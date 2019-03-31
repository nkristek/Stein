using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services.InstallService;
using Stein.ViewModels.Commands.MainWindowViewModelCommands;
using Stein.ViewModels.Extensions;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class InstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        private readonly IInstallService _installService;

        public InstallApplicationCommand(IDialogService dialogService, IViewModelService viewModelService, IInstallService installService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(ApplicationViewModel.Parent), nameof(ApplicationViewModel.SelectedInstallerBundle), nameof(ApplicationViewModel.IsUpdating))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowViewModel mainWindowViewModel) || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any() && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowViewModel mainWindowViewModel))
                return;

            var installers = viewModel.SelectedInstallerBundle.Installers;
            if (!installers.Any())
                return;

            mainWindowViewModel.CurrentInstallation = _viewModelService.CreateViewModel<InstallationViewModel>(mainWindowViewModel);
            mainWindowViewModel.CurrentInstallation.Name = viewModel.Name;
            mainWindowViewModel.CurrentInstallation.TotalInstallerFileCount = installers.Count;
            
            try
            {
                mainWindowViewModel.RecentInstallationResult = await ViewModelInstallService.Install(
                    _viewModelService,
                    _installService,
                    mainWindowViewModel.CurrentInstallation,
                    installers,
                    viewModel.EnableSilentInstallation, 
                    viewModel.DisableReboot, 
                    viewModel.EnableInstallationLogging, 
                    viewModel.AutomaticallyDeleteInstallationLogs, 
                    viewModel.KeepNewestInstallationLogs,
                    viewModel.FilterDuplicateInstallers);
            }
            finally
            {
                mainWindowViewModel.CurrentInstallation.Dispose();
                mainWindowViewModel.CurrentInstallation = null;
            }
            
            var refreshCommand = mainWindowViewModel.GetCommand<MainWindowViewModel, RefreshApplicationsCommand>();
            if (refreshCommand != null)
                await refreshCommand.ExecuteAsync(null);
        }
    }
}
