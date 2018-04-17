using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.Localizations;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public class SelectFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        public SelectFolderCommand(ApplicationDialogModel parent) : base(parent) { }

        protected override void DoExecute(ApplicationDialogModel viewModel, object parameter)
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

        protected override void OnThrownException(ApplicationDialogModel viewModel, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.Instance.ShowError(exception);
        }
    }
}
