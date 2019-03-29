using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.InstallationViewModelCommands
{
    public sealed class CancelOperationCommand
        : ViewModelCommand<InstallationViewModel>
    {
        [CanExecuteSource(nameof(InstallationViewModel.State))]
        protected override bool CanExecute(InstallationViewModel viewModel, object parameter)
        {
            return viewModel.State != InstallationState.Cancelled 
                && viewModel.State != InstallationState.Finished;
        }

        protected override void Execute(InstallationViewModel viewModel, object parameter)
        {
            viewModel.State = InstallationState.Cancelled;
        }
    }
}
