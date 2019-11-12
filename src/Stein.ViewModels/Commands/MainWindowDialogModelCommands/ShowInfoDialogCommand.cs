using System;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowDialogModelCommands
{
    public sealed class ShowInfoDialogCommand
        : ViewModelCommand<MainWindowDialogModel>
    {
        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public ShowInfoDialogCommand(IDialogService dialogService, IViewModelService viewModelService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        /// <inheritdoc />
        protected override void Execute(MainWindowDialogModel? viewModel, object? parameter)
        {
            if (viewModel == null)
                return;

            var dialogModel = _viewModelService.CreateViewModel<AboutDialogModel>(viewModel);
            _dialogService.ShowDialog(dialogModel);
        }
    }
}
