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
            var applicationCopy = new ApplicationDialogModel(viewModel)
            {
                FolderId = viewModel.FolderId,
                Name = viewModel.Name,
                Path = viewModel.Path,
                EnableSilentInstallation = viewModel.EnableSilentInstallation,
                EnableInstallationLogging = viewModel.EnableInstallationLogging
            };
            if (DialogService.ShowDialog(applicationCopy, Strings.EditFolder) != true)
                return;

            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == applicationCopy.FolderId);
            if (associatedApplicationFolder == null)
                return;

            associatedApplicationFolder.Name = applicationCopy.Name;
            associatedApplicationFolder.Path = applicationCopy.Path;
            associatedApplicationFolder.EnableSilentInstallation = applicationCopy.EnableSilentInstallation;
            associatedApplicationFolder.EnableInstallationLogging = applicationCopy.EnableInstallationLogging;

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
