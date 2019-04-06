using System;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowDialogModelCommands
{
    public sealed class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowDialogModel>
    {
        private readonly IViewModelService _viewModelService;

        public RefreshApplicationsCommand(IViewModelService viewModelService) 
        {
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
            await _viewModelService.UpdateViewModelAsync(viewModel);
        }
    }
}
