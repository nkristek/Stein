using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.UpdateDialogModelCommands
{
    public class CancelUpdateCommand
        : ViewModelCommand<UpdateDialogModel>
    {
        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(UpdateDialogModel.IsUpdateDownloading))
                || e.PropertyName.Equals(nameof(UpdateDialogModel.IsUpdateCancelled)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(UpdateDialogModel? viewModel, object? parameter)
        {
            return viewModel != null && viewModel.IsUpdateDownloading && !viewModel.IsUpdateCancelled;
        }

        /// <inheritdoc />
        protected override void Execute(UpdateDialogModel? viewModel, object? parameter)
        {
            (viewModel?.InstallUpdateCommand as InstallUpdateCommand)?.Cancel();
        }
    }
}
