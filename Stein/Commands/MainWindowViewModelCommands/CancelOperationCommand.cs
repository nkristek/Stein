using System;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.MainWindowViewModelCommands
{
    public class CancelOperationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public CancelOperationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation != null 
                && viewModel.CurrentInstallation.State != InstallationState.Preparing 
                && viewModel.CurrentInstallation.State != InstallationState.Cancelled;
        }

        protected override void ExecuteSync(MainWindowViewModel viewModel, object view, object parameter)
        {
            viewModel.CurrentInstallation.State = InstallationState.Cancelled;
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
        }
    }
}
