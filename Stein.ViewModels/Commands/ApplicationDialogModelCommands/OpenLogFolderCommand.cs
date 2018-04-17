using System;
using System.Diagnostics;
using System.IO;
using nkristek.MVVMBase.Commands;
using Stein.Services;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public class OpenLogFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        public OpenLogFolderCommand(ApplicationDialogModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationDialogModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(LogService.LogFolderPath) && Directory.Exists(LogService.LogFolderPath);
        }

        protected override void DoExecute(ApplicationDialogModel viewModel, object parameter)
        {
            Process.Start(LogService.LogFolderPath);
        }

        protected override void OnThrownException(ApplicationDialogModel viewModel, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.Instance.ShowError(exception);
        }
    }
}
