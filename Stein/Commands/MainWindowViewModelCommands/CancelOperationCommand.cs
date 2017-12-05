using System;
using Stein.ViewModels;
using WpfBase.Commands;
using Stein.Services;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class CancelOperationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public CancelOperationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation != null 
                && viewModel.CurrentInstallation.State != InstallationState.Preparing 
                && viewModel.CurrentInstallation.State != InstallationState.Cancelled;
        }

        public override void Execute(MainWindowViewModel viewModel, object view, object parameter)
        {
            viewModel.CurrentInstallation.State = InstallationState.Cancelled;
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
        }
    }
}
