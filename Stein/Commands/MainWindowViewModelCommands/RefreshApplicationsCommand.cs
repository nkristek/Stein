using System;
using System.Threading.Tasks;
using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using System.Windows;
using System.Linq;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public RefreshApplicationsCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            // save changes from application viewmodels back to the configuration
            foreach (var changedApplication in viewModel.Applications.Where(application => application.IsDirty))
                ViewModelService.SaveViewModel(changedApplication);
            
            // get new installers
            await ConfigurationService.SyncApplicationFoldersWithDiskAsync();
            await ConfigurationService.SaveConfigurationToDiskAsync();
            await InstallService.RefreshInstalledProgramsAsync();

            // update the viewmodels
            var createdOrUpdatedApplications = ViewModelService.CreateOrUpdateApplicationViewModels(viewModel, viewModel.Applications.ToList());
            viewModel.Applications.Clear();
            foreach (var application in createdOrUpdatedApplications)
                viewModel.Applications.Add(application);
            viewModel.IsDirty = false;
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}
