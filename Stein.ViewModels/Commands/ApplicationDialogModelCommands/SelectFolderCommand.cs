using System;
using log4net;
using Microsoft.WindowsAPICodePack.Dialogs;
using nkristek.MVVMBase.Commands;
using Stein.Localizations;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public class SelectFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public SelectFolderCommand(ApplicationDialogModel parent, IDialogService dialogService) : base(parent)
        {
            _dialogService = dialogService;
        }

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
            Log.Error(exception);
            _dialogService.ShowError(exception);
        }
    }
}
