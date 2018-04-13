using System;
using System.Linq;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.ConfigurationTypes;
using nkristek.Stein.Localizations;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;


namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class EditApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public EditApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var applicationDialog = new ApplicationDialogModel(viewModel)
            {
                FolderId = viewModel.FolderId,
                Name = viewModel.Name,
                Path = viewModel.Path,
                EnableSilentInstallation = viewModel.EnableSilentInstallation,
                DisableReboot = viewModel.DisableReboot,
                EnableInstallationLogging = viewModel.EnableInstallationLogging
            };
            if (DialogService.ShowDialog(applicationDialog, Strings.EditFolder) != true)
                return;

            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == applicationDialog.FolderId);
            if (associatedApplicationFolder == null)
                return;

            associatedApplicationFolder.Name = applicationDialog.Name;
            associatedApplicationFolder.Path = applicationDialog.Path;
            associatedApplicationFolder.EnableSilentInstallation = applicationDialog.EnableSilentInstallation;
            associatedApplicationFolder.DisableReboot = applicationDialog.DisableReboot;
            associatedApplicationFolder.EnableInstallationLogging = applicationDialog.EnableInstallationLogging;

            await associatedApplicationFolder.SyncWithDiskAsync();
            await ConfigurationService.SaveConfigurationToDiskAsync();

            ViewModelService.UpdateViewModel(viewModel);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.ShowErrorDialog(exception);
            (viewModel.FirstParentOfType<MainWindowViewModel>())?.RefreshApplicationsCommand.Execute(null);
        }
    }
}
