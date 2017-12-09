using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.ConfigurationTypes;
using nkristek.Stein.Localizations;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;


namespace nkristek.Stein.Commands.MainWindowViewModelCommands
{
    public class EditApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public EditApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            var applicationToEdit = parameter as ApplicationViewModel;
            if (applicationToEdit == null)
                return;

            var applicationCopy = new ApplicationViewModel(viewModel)
            {
                FolderId = applicationToEdit.FolderId,
                Name = applicationToEdit.Name,
                Path = applicationToEdit.Path,
                EnableSilentInstallation = applicationToEdit.EnableSilentInstallation,
                EnableInstallationLogging = applicationToEdit.EnableInstallationLogging
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

            ViewModelService.UpdateViewModel(applicationToEdit);
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
            viewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
