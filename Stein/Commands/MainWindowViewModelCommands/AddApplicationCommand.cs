using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using WpfBase.Extensions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Stein.Commands.MainWindowViewModelCommands
{
    class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public AddApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            string selectedPath;

            // this is the standard select folder dialog but it is very limited in functionality and ugly
            //using (var dialog = new FolderBrowserDialog())
            //{
            //    var result = dialog.ShowDialog();
            //    if (result != DialogResult.OK)
            //        return;

            //    selectedPath = dialog.SelectedPath;
            //}

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;

                var result = dialog.ShowDialog();
                if (result != CommonFileDialogResult.Ok)
                    return;

                selectedPath = dialog.FileName;
            }

            if (String.IsNullOrWhiteSpace(selectedPath) || selectedPath.ContainsInvalidPathChars() || !Directory.Exists(selectedPath))
            {
                MessageBox.Show("Selected path is not valid!");
                return;
            }

            var applicationFolder = new ApplicationFolder()
            {
                Id = Guid.NewGuid(),
                Name = new DirectoryInfo(selectedPath).Name,
                Path = selectedPath,
                EnableSilentInstallation = true
            };
            await ConfigurationService.SyncApplicationFolderWithDiskAsync(applicationFolder);

            ConfigurationService.Configuration.ApplicationFolders.Add(applicationFolder);
            
            await ConfigurationService.SaveConfigurationToDiskAsync();
            
            viewModel.Applications.Add(ViewModelService.CreateApplicationViewModel(applicationFolder, viewModel));
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            MessageBox.Show(exception.Message);
            viewModel.RefreshApplicationsCommand.ExecuteAsync(null).Wait();
        }
    }
}
