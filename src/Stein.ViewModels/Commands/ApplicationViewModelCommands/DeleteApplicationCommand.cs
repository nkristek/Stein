using System;
using System.ComponentModel;
using System.Threading.Tasks;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

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
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null 
                || String.IsNullOrEmpty(e.PropertyName) 
                || e.PropertyName.Equals(nameof(ApplicationViewModel.IsUpdating)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(ApplicationViewModel? viewModel, object? parameter)
        {
            return viewModel?.Parent is MainWindowDialogModel parent && parent.CurrentInstallation == null && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(ApplicationViewModel? viewModel, object? parameter)
        {
            if (viewModel == null)
                return;
                
            await _viewModelService.DeleteViewModelAsync(viewModel);

            if (viewModel?.Parent is MainWindowDialogModel parent)
                parent.Applications.Remove(viewModel);
        }
    }
}
