using Stein.Services;
using Stein.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using WpfBase.Commands;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class OpenLogFolderCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public OpenLogFolderCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            return !String.IsNullOrEmpty(InstallService.InstallationLogFolderPath);
        }

        public override void Execute(ApplicationViewModel viewModel, object view, object parameter)
        {
            Process.Start(InstallService.InstallationLogFolderPath);
        }

        public override void OnThrownExeption(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
        }
    }
}
