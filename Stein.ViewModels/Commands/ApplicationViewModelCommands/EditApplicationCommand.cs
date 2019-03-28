﻿using System;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class EditApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public EditApplicationCommand(IDialogService dialogService, IViewModelService viewModelService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        [CanExecuteSource(nameof(ApplicationViewModel.Parent))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object parameter)
        {
            try
            {
                var dialogModel = _viewModelService.CreateViewModel<ApplicationDialogModel>(viewModel);
                if (_dialogService.ShowDialog(dialogModel) != true)
                    return;

                await _viewModelService.SaveViewModelAsync(dialogModel);
                await _viewModelService.UpdateViewModelAsync(viewModel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
