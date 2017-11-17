using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class CancelOperationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public CancelOperationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation != null 
                && viewModel.CurrentInstallation.State != InstallationViewModel.InstallationState.Preparing 
                && viewModel.CurrentInstallation.State != InstallationViewModel.InstallationState.Cancelled;
        }
        public override void Execute(MainWindowViewModel viewModel, object view, object parameter)
        {
            viewModel.CurrentInstallation.State = InstallationViewModel.InstallationState.Cancelled;
        }
    }
}
