using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.ConfigurationTypes;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using WpfBase.Extensions;
using Stein.Localizations;

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
            
            if (DialogService.ShowDialog(application, Strings.AddFolder) != true)
                return;

            if (String.IsNullOrWhiteSpace(application.Path) || application.Path.ContainsInvalidPathChars() || !Directory.Exists(application.Path))
            {
                MessageBox.Show(Strings.SelectedPathNotValid);
                return;
            }

            var applicationFolder = new ApplicationFolder()
            {
                Id = Guid.NewGuid(),
                Name = application.Name,
                Path = application.Path,
                EnableSilentInstallation = application.EnableSilentInstallation
            };

            await applicationFolder.SyncWithDiskAsync();
            ConfigurationService.Configuration.ApplicationFolders.Add(applicationFolder);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            application.FolderId = applicationFolder.Id;
            ViewModelService.UpdateViewModel(application);
            viewModel.Applications.Add(application);
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
            Task.Run(async () =>
            {
                await viewModel.RefreshApplicationsCommand.ExecuteAsync(null);
            });
        }
    }
}
