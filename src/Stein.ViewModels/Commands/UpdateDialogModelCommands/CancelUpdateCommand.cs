using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Extensions;

namespace Stein.ViewModels.Commands.UpdateDialogModelCommands
{
    public class CancelUpdateCommand
        : ViewModelCommand<UpdateDialogModel>
    {
        /// <inheritdoc />
        [CanExecuteSource(nameof(UpdateDialogModel.IsUpdateDownloading), nameof(UpdateDialogModel.IsUpdateCancelled))]
        protected override bool CanExecute(UpdateDialogModel viewModel, object parameter)
        {
            return viewModel.IsUpdateDownloading && !viewModel.IsUpdateCancelled;
        }

        /// <inheritdoc />
        protected override void Execute(UpdateDialogModel viewModel, object parameter)
        {
            var installCommand = viewModel.GetCommand<UpdateDialogModel, InstallUpdateCommand>();
            installCommand?.Cancel();
        }
    }
}
