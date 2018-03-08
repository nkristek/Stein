using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.ConfigurationTypes;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;
using nkristek.Stein.Localizations;

namespace nkristek.Stein.Commands.MainWindowViewModelCommands
{
    public class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public RefreshApplicationsCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            // save changes from application viewmodels back to the configuration
            foreach (var changedApplication in viewModel.Applications.Where(application => application.IsDirty))
                ViewModelService.SaveViewModel(changedApplication);

            // get new installers
            foreach (var applicationFolder in ConfigurationService.Configuration.ApplicationFolders)
            {
                try
                {
                    await applicationFolder.SyncWithDiskAsync();
                }
                catch (Exception exception)
                {
                    applicationFolder.SubFolders.Clear();
                    MessageBox.Show(String.Format(Strings.RefreshFailed, applicationFolder.Path, exception.Message));
                }
            }
            await ConfigurationService.SaveConfigurationToDiskAsync();
            await InstallService.RefreshInstalledProgramsAsync();

            // update the viewmodels
            var applications = ViewModelService.CreateOrUpdateApplicationViewModels(viewModel, viewModel.Applications.ToList());
            viewModel.Applications.Clear();
            foreach (var application in applications)
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
        }
    }
}
