using System;
using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class CancelOperationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public CancelOperationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation != null 
                && viewModel.CurrentInstallation.State != InstallationState.Preparing 
                && viewModel.CurrentInstallation.State != InstallationState.Cancelled;
        }

        protected override void DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            viewModel.CurrentInstallation.State = InstallationState.Cancelled;
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object parameter, Exception exception)
        {
            LogService.LogError(exception);
        }
    }
}
