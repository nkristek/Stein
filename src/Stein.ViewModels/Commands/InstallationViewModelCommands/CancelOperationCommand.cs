using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.InstallationViewModelCommands
{
    public sealed class CancelOperationCommand
        : ViewModelCommand<InstallationViewModel>
    {
        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(InstallationViewModel.State)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(InstallationViewModel? viewModel, object? parameter)
        {
            return viewModel != null
                && viewModel.State != InstallationState.Cancelled 
                && viewModel.State != InstallationState.Finished;
        }

        /// <inheritdoc />
        protected override void Execute(InstallationViewModel? viewModel, object? parameter)
        {
            if (viewModel != null)
                viewModel.State = InstallationState.Cancelled;
        }
    }
}
