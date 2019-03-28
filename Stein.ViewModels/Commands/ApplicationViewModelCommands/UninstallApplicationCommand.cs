using System;
using System.Linq;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services.InstallService;
using Stein.ViewModels.Commands.MainWindowViewModelCommands;
using Stein.ViewModels.Extensions;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class UninstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        private readonly IInstallService _installService;

        private readonly IProgressBarService _progressBarService;

        public UninstallApplicationCommand(IDialogService dialogService, IViewModelService viewModelService, IInstallService installService, IProgressBarService progressBarService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
            _progressBarService = progressBarService ?? throw new ArgumentNullException(nameof(progressBarService));
        }

        [CanExecuteSource(nameof(ApplicationViewModel.Parent), nameof(ApplicationViewModel.SelectedInstallerBundle))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowViewModel mainWindowViewModel) || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any(i => i.IsInstalled == true);
        }
        
        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowViewModel mainWindowViewModel))
                return;

            var installers = viewModel.SelectedInstallerBundle.Installers;
            mainWindowViewModel.CurrentInstallation = _viewModelService.CreateViewModel<InstallationViewModel>(mainWindowViewModel);
            mainWindowViewModel.CurrentInstallation.Name = viewModel.Name;
            mainWindowViewModel.CurrentInstallation.InstallerCount = installers.Count;

            try
            {
                var installationResult = await ViewModelInstallService.Uninstall(
                    _viewModelService,
                    _installService,
                    _progressBarService,
                    mainWindowViewModel.CurrentInstallation,
                    installers,
                    viewModel.EnableSilentInstallation,
                    viewModel.DisableReboot,
                    viewModel.EnableInstallationLogging,
                    viewModel.AutomaticallyDeleteInstallationLogs,
                    viewModel.KeepNewestInstallationLogs);

                Task refreshTask = null;
                var refreshCommand = mainWindowViewModel.GetCommand<MainWindowViewModel, RefreshApplicationsCommand>();
                if (refreshCommand != null)
                    refreshTask = refreshCommand.ExecuteAsync(null);

                _dialogService.ShowDialog(installationResult);

                if (refreshTask != null)
                    await refreshTask;
            }
            finally
            {
                mainWindowViewModel.CurrentInstallation = null;
            }
        }
    }
}
