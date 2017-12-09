using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.Extensions;
using nkristek.Stein.Localizations;
using nkristek.Stein.ConfigurationTypes;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.MainWindowViewModelCommands
{
    public class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public AddApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
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

        protected override void OnThrownException(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
            viewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
