using Microsoft.WindowsAPICodePack.Dialogs;
using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class SelectFolderCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public SelectFolderCommand(ApplicationViewModel parent) : base(parent) { }

        public override void Execute(ApplicationViewModel viewModel, object view, object parameter)
        {
            // this is the standard select folder dialog but it is very limited in functionality and ugly
            //using (var dialog = new FolderBrowserDialog())
            //{
            //    var result = dialog.ShowDialog();
            //    if (result != DialogResult.OK)
            //        return;

            //    viewModel.Path = dialog.SelectedPath;
            //}

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;

                var result = dialog.ShowDialog();
                if (result != CommonFileDialogResult.Ok)
                    return;

                viewModel.Path = dialog.FileName;
            }
        }
    }
}
