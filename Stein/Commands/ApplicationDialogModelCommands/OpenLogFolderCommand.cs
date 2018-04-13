using System;
using System.Diagnostics;
using System.IO;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationDialogModelCommands
{
    public class OpenLogFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        public OpenLogFolderCommand(ApplicationDialogModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationDialogModel viewModel, object view, object parameter)
        {
            return !String.IsNullOrEmpty(InstallService.InstallationLogFolderPath) && Directory.Exists(InstallService.InstallationLogFolderPath);
        }

        protected override void ExecuteSync(ApplicationDialogModel viewModel, object view, object parameter)
        {
            Process.Start(InstallService.InstallationLogFolderPath);
        }

        protected override void OnThrownException(ApplicationDialogModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.ShowErrorDialog(exception);
        }
    }
}
