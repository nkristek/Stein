﻿using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using WpfBase.Commands;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class EditApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public EditApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
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
            };
            if (DialogService.ShowDialog(applicationCopy, "Edit folder") != true)
                return;

            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == applicationCopy.FolderId);
            if (associatedApplicationFolder == null)
                return;

            associatedApplicationFolder.Name = applicationCopy.Name;
            associatedApplicationFolder.Path = applicationCopy.Path;
            associatedApplicationFolder.EnableSilentInstallation = applicationCopy.EnableSilentInstallation;

            await ConfigurationService.SyncApplicationFolderWithDiskAsync(associatedApplicationFolder);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            ViewModelService.UpdateViewModel(applicationToEdit);
        }
    }
}
