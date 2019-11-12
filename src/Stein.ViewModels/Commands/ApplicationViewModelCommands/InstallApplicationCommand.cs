using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using NKristek.Smaragd.Commands;
using Stein.Common.InstallService;
using Stein.Localization;
using Stein.Presentation;
using Stein.ViewModels.Commands.MainWindowDialogModelCommands;
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

        private readonly INotificationService _notificationService;

        private readonly IUriService _uriService;

        private readonly string _downloadFolderPath;

        public InstallApplicationCommand(IDialogService dialogService, IViewModelService viewModelService, IInstallService installService, INotificationService notificationService, IUriService uriService, string downloadFolderPath)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            _downloadFolderPath = !String.IsNullOrEmpty(downloadFolderPath) ? downloadFolderPath : throw new ArgumentNullException(nameof(downloadFolderPath));
        }

        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(ApplicationViewModel.SelectedInstallerBundle))
                || e.PropertyName.Equals(nameof(ApplicationViewModel.IsUpdating)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(ApplicationViewModel? viewModel, object? parameter)
        {
            if (!(viewModel?.Parent is MainWindowDialogModel mainWindowDialogModel) || mainWindowDialogModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any() && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(ApplicationViewModel? viewModel, object? parameter)
        {
            if (!(viewModel?.Parent is MainWindowDialogModel mainWindowDialogModel))
                return;

            var installers = viewModel.SelectedInstallerBundle?.Installers;
            if (installers == null || !installers.Any())
                return;

            mainWindowDialogModel.CurrentInstallation = _viewModelService.CreateViewModel<InstallationViewModel>(mainWindowDialogModel);
            mainWindowDialogModel.CurrentInstallation.Name = viewModel.Name;
            mainWindowDialogModel.CurrentInstallation.TotalInstallerFileCount = installers.Count;
            
            try
            {
                mainWindowDialogModel.RecentInstallationResult = await ViewModelInstallService.Install(
                    _viewModelService,
                    _installService,
                    _notificationService,
                    _uriService,
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

            mainWindowDialogModel.RefreshApplicationsCommand?.Execute(null);

            if (mainWindowDialogModel.RecentInstallationResult.InstallationResults.Any(r => r.State != InstallationResultState.Success && r.State != InstallationResultState.Skipped))
                _dialogService.ShowDialog(mainWindowDialogModel.RecentInstallationResult);
            else
                _notificationService.ShowSuccess(Strings.OperationSuccessfull, () => _dialogService.ShowDialog(mainWindowDialogModel.RecentInstallationResult));
        }
    }
}
