using System;
using System.Diagnostics;
using System.Windows;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class OpenLogFolderCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public OpenLogFolderCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            return !String.IsNullOrEmpty(InstallService.InstallationLogFolderPath);
        }

        protected override void ExecuteSync(ApplicationViewModel viewModel, object view, object parameter)
        {
            Process.Start(InstallService.InstallationLogFolderPath);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
        }
    }
}
