using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.Localizations;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class SelectFolderCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public SelectFolderCommand(ApplicationViewModel parent) : base(parent) { }

        protected override void ExecuteSync(ApplicationViewModel viewModel, object view, object parameter)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = Strings.SelectFolder;
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;
                
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return;

                viewModel.Path = dialog.FileName;
            }
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
        }
    }
}
