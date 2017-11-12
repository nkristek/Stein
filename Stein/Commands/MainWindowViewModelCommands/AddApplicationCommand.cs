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
            var application = new ApplicationViewModel(viewModel)
            {
                FolderId = Guid.NewGuid(),
                EnableSilentInstallation = true
            };

            if (DialogService.ShowDialog(application) != true)
                return;

            if (String.IsNullOrWhiteSpace(application.Path) || application.Path.ContainsInvalidPathChars() || !Directory.Exists(application.Path))
            {
                MessageBox.Show("Selected path is not valid!");
                return;
            }

            var applicationFolder = new ApplicationFolder()
            {
                Id = application.FolderId,
                Name = application.Name,
                Path = application.Path,
                EnableSilentInstallation = application.EnableSilentInstallation
            };

            await ConfigurationService.SyncApplicationFolderWithDiskAsync(applicationFolder);
            
            ConfigurationService.Configuration.ApplicationFolders.Add(applicationFolder);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            ViewModelService.UpdateApplicationViewModel(application);
            viewModel.Applications.Add(application);
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            MessageBox.Show(exception.Message);
            viewModel.RefreshApplicationsCommand.ExecuteAsync(null).Wait();
        }
    }
}
