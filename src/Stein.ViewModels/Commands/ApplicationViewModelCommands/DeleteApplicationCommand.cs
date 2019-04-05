using System;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class DeleteApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private readonly IViewModelService _viewModelService;

        public DeleteApplicationCommand(IViewModelService viewModelService)
        {
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
            if (!(viewModel.Parent is MainWindowDialogModel parent))
                return;

            await _viewModelService.DeleteViewModelAsync(viewModel);
            parent.Applications.Remove(viewModel);
        }
    }
}
