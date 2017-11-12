using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBase.Commands;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class EditApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public EditApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var application = new ApplicationViewModel(viewModel)
            {
                FolderId = viewModel.FolderId,
                Name = viewModel.Name,
                Path = viewModel.Path,
                EnableSilentInstallation = viewModel.EnableSilentInstallation,
            };
            if (DialogService.ShowDialog(application) != true)
                return;

            var associatedApplicationFolder = ConfigurationService.Configuration.ApplicationFolders.FirstOrDefault(af => af.Id == application.FolderId);
            if (associatedApplicationFolder == null)
                return;

            associatedApplicationFolder.Name = application.Name;
            associatedApplicationFolder.Path = application.Path;
            associatedApplicationFolder.EnableSilentInstallation = application.EnableSilentInstallation;

            await ConfigurationService.SyncApplicationFolderWithDiskAsync(associatedApplicationFolder);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            ViewModelService.UpdateApplicationViewModel(viewModel);
        }
    }
}
