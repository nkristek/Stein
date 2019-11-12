using System;
using System.Threading.Tasks;
using System.ComponentModel;
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
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(MainWindowDialogModel.CurrentInstallation))
                || e.PropertyName.Equals(nameof(MainWindowDialogModel.IsUpdating)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(MainWindowDialogModel? viewModel, object? parameter)
        {
            return viewModel != null && viewModel.CurrentInstallation == null && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(MainWindowDialogModel? viewModel, object? parameter)
        {
            if (viewModel == null)
                return;

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
