using System;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class EditApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public EditApplicationCommand(IDialogService dialogService, IViewModelService viewModelService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(ApplicationViewModel.Parent), nameof(ApplicationViewModel.IsUpdating))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return viewModel.Parent is MainWindowDialogModel parent && parent.CurrentInstallation == null && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object parameter)
        {
            var dialogModel = _viewModelService.CreateViewModel<ApplicationDialogModel>(viewModel);
            if (_dialogService.ShowDialog(dialogModel) != true)
                return;

            await _viewModelService.SaveViewModelAsync(dialogModel);
            await _viewModelService.UpdateViewModelAsync(viewModel);
        }
    }
}
