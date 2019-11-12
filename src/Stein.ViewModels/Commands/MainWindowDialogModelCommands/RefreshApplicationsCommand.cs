using System;
using System.Threading.Tasks;
using System.ComponentModel;
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
            if (viewModel != null)
                await _viewModelService.UpdateViewModelAsync(viewModel);
        }
    }
}
