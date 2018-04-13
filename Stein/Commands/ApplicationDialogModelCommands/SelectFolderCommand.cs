using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.Localizations;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationDialogModelCommands
{
    public class SelectFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        public SelectFolderCommand(ApplicationDialogModel parent) : base(parent) { }

        protected override void ExecuteSync(ApplicationDialogModel viewModel, object view, object parameter)
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

        protected override void OnThrownException(ApplicationDialogModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.ShowErrorDialog(exception);
        }
    }
}
