using System;
using System.Linq;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services.InstallService;
using Stein.ViewModels.Commands.MainWindowDialogModelCommands;
using Stein.ViewModels.Extensions;
using Stein.ViewModels.Services;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class InstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        private readonly IInstallService _installService;

        private readonly string _downloadFolderPath;

        public InstallApplicationCommand(IDialogService dialogService, IViewModelService viewModelService, IInstallService installService, string downloadFolderPath)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
            _downloadFolderPath = !String.IsNullOrEmpty(downloadFolderPath) ? downloadFolderPath : throw new ArgumentNullException(nameof(downloadFolderPath));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(ApplicationViewModel.Parent), nameof(ApplicationViewModel.SelectedInstallerBundle), nameof(ApplicationViewModel.IsUpdating))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowDialogModel mainWindowDialogModel) || mainWindowDialogModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any() && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowDialogModel mainWindowDialogModel))
                return;

            var installers = viewModel.SelectedInstallerBundle.Installers;
            if (!installers.Any())
                return;

            mainWindowDialogModel.CurrentInstallation = _viewModelService.CreateViewModel<InstallationViewModel>(mainWindowDialogModel);
            mainWindowDialogModel.CurrentInstallation.Name = viewModel.Name;
            mainWindowDialogModel.CurrentInstallation.TotalInstallerFileCount = installers.Count;
            
            try
            {
                mainWindowDialogModel.RecentInstallationResult = await ViewModelInstallService.Install(
                    _viewModelService,
                    _installService,
                    mainWindowDialogModel.CurrentInstallation,
                    installers,
                    viewModel.EnableSilentInstallation, 
                    viewModel.DisableReboot, 
                    viewModel.EnableInstallationLogging, 
                    viewModel.AutomaticallyDeleteInstallationLogs, 
                    viewModel.KeepNewestInstallationLogs,
                    viewModel.FilterDuplicateInstallers,
                    _downloadFolderPath);
            }
            finally
            {
                mainWindowDialogModel.CurrentInstallation.Dispose();
                mainWindowDialogModel.CurrentInstallation = null;
            }

            Task refreshTask = null;
            if (mainWindowDialogModel.TryGetCommand<MainWindowDialogModel, RefreshApplicationsCommand>(out var refreshCommand))
                refreshTask = refreshCommand.ExecuteAsync(null);

            if (mainWindowDialogModel.RecentInstallationResult.InstallationResults.Any(r => r.State != InstallationResultState.Success || r.State != InstallationResultState.Skipped))
                _dialogService.ShowDialog(mainWindowDialogModel.RecentInstallationResult);

            if (refreshTask != null)
                await refreshTask;
        }
    }
}
