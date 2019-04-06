using System;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Localization;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowDialogModelCommands
{
    public sealed class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowDialogModel>
    {
        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public AddApplicationCommand(IDialogService dialogService, IViewModelService viewModelService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(MainWindowDialogModel.CurrentInstallation), nameof(MainWindowDialogModel.IsUpdating))]
        protected override bool CanExecute(MainWindowDialogModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(MainWindowDialogModel viewModel, object parameter)
        {
            var dialogModel = _viewModelService.CreateViewModel<ApplicationDialogModel>(viewModel);
            do
            {
                if (_dialogService.ShowDialog(dialogModel) != true)
                    return;

                if (!dialogModel.IsValid)
                    _dialogService.ShowErrorDialog(Strings.DialogInputNotValid, Strings.Error);
            } while (!dialogModel.IsValid);

            await _viewModelService.SaveViewModelAsync(dialogModel);
            await _viewModelService.UpdateViewModelAsync(viewModel);
        }
    }
}
