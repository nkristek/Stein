﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services.InstallService;
using Stein.ViewModels.Commands.MainWindowDialogModelCommands;
using Stein.ViewModels.Extensions;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class InstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private readonly IViewModelService _viewModelService;

        private readonly IInstallService _installService;

        public InstallApplicationCommand(IViewModelService viewModelService, IInstallService installService)
        {
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
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
                    viewModel.FilterDuplicateInstallers);
            }
            finally
            {
                mainWindowDialogModel.CurrentInstallation.Dispose();
                mainWindowDialogModel.CurrentInstallation = null;
            }
            
            var refreshCommand = mainWindowDialogModel.GetCommand<MainWindowDialogModel, RefreshApplicationsCommand>();
            if (refreshCommand != null)
                await refreshCommand.ExecuteAsync(null);
        }
    }
}
