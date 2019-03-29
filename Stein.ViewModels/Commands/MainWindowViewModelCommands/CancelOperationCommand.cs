using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class CancelOperationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation != null && !viewModel.CurrentInstallation.CancellationTokenSource.IsCancellationRequested;
        }

        protected override void Execute(MainWindowViewModel viewModel, object parameter)
        {
            viewModel.CurrentInstallation.State = InstallationState.Cancelled;
        }
    }
}
