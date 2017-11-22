using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using Stein.ViewModels;
using WpfBase.Commands;
using Stein.Services;
using System.Windows;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class SelectFolderCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public SelectFolderCommand(ApplicationViewModel parent) : base(parent) { }

        public override void Execute(ApplicationViewModel viewModel, object view, object parameter)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;
                
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return;

                viewModel.Path = dialog.FileName;
            }
        }

        public override void OnThrownExeption(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
        }
    }
}
