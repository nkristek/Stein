using System;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class DeleteApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public DeleteApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            ConfigurationService.Configuration.ApplicationFolders.RemoveAll(af => af.Id == viewModel.FolderId);
            await ConfigurationService.SaveConfigurationToDiskAsync();

            (viewModel.Parent as MainWindowViewModel)?.Applications.Remove(viewModel);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.ShowErrorDialog(exception);
            (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.Execute(null);
        }
    }
}
