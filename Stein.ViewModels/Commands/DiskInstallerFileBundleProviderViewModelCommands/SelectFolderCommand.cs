using System;
using log4net;
using Microsoft.WindowsAPICodePack.Dialogs;
using NKristek.Smaragd.Commands;
using Stein.Localizations;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.DiskInstallerFileBundleProviderViewModelCommands
{
    public sealed class SelectFolderCommand
        : ViewModelCommand<DiskInstallerFileBundleProviderViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public SelectFolderCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        protected override void Execute(DiskInstallerFileBundleProviderViewModel viewModel, object parameter)
        {
            try
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
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
