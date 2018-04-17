using System;
using System.Linq;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.Localizations;
using Stein.Services.Extensions;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public RefreshApplicationsCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            // save changes from application viewmodels back to the configuration
            foreach (var changedApplication in viewModel.Applications.Where(application => application.IsDirty))
                ViewModelService.Instance.SaveViewModel(changedApplication);

            // get new installers
            foreach (var applicationFolder in ConfigurationService.Instance.Configuration.ApplicationFolders)
            {
                try
                {
                    await applicationFolder.SyncWithDiskAsync();
                }
                catch (Exception exception)
                {
                    applicationFolder.SubFolders.Clear();
                    DialogService.Instance.ShowMessage(String.Format(Strings.RefreshFailed, applicationFolder.Path, exception.Message));
                }
            }
            await ConfigurationService.Instance.SaveConfigurationToDiskAsync();
            await Task.Run(() => InstallService.Instance.RefreshInstalledPrograms());

            // update the viewmodels
            var applications = ViewModelService.Instance.CreateViewModels(viewModel, viewModel.Applications.ToList());
            viewModel.Applications.Clear();
            foreach (var application in applications)
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.Instance.ShowError(exception);
        }
    }
}
