using System;
using System.Threading.Tasks;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using System.Windows;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class DeleteApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public DeleteApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            var applicationToDelete = parameter as ApplicationViewModel;
            if (applicationToDelete == null)
                return;
            
            ConfigurationService.Configuration.ApplicationFolders.RemoveAll(af => af.Id == applicationToDelete.FolderId);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            viewModel.Applications.Remove(applicationToDelete);
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
            viewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
