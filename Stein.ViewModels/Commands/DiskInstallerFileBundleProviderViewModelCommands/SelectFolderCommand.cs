using Microsoft.WindowsAPICodePack.Dialogs;
using NKristek.Smaragd.Commands;
using Stein.Localizations;

namespace Stein.ViewModels.Commands.DiskInstallerFileBundleProviderViewModelCommands
{
    public sealed class SelectFolderCommand
        : ViewModelCommand<DiskInstallerFileBundleProviderViewModel>
    {
        protected override void Execute(DiskInstallerFileBundleProviderViewModel viewModel, object parameter)
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
    }
}
