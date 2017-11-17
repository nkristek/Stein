using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using WpfBase.Extensions;

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
                EnableSilentInstallation = true
            };
            
            if (DialogService.ShowDialog(application, "Add folder") != true)
                return;

            if (String.IsNullOrWhiteSpace(application.Path) || application.Path.ContainsInvalidPathChars() || !Directory.Exists(application.Path))
            {
                MessageBox.Show("Selected path is not valid!");
                return;
            }

            var applicationFolder = new ApplicationFolder()
            {
                Id = Guid.NewGuid(),
                Name = application.Name,
                Path = application.Path,
                EnableSilentInstallation = application.EnableSilentInstallation
            };

            await ConfigurationService.SyncApplicationFolderWithDiskAsync(applicationFolder);
            
            ConfigurationService.Configuration.ApplicationFolders.Add(applicationFolder);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            application.FolderId = applicationFolder.Id;
            ViewModelService.UpdateViewModel(application);
            viewModel.Applications.Add(application);
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            MessageBox.Show(exception.Message);
            viewModel.RefreshApplicationsCommand.ExecuteAsync(null).Wait();
        }
    }
}
