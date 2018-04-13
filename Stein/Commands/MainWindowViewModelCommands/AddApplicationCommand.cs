using System;
using System.IO;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.Extensions;
using nkristek.Stein.Localizations;
using nkristek.Stein.ConfigurationTypes;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.MainWindowViewModelCommands
{
    public class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public AddApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            var applicationDialog = new ApplicationDialogModel(viewModel)
            {
                EnableSilentInstallation = true,
                DisableReboot = true
            };
            
            if (DialogService.ShowDialog(applicationDialog, Strings.AddFolder) != true)
                return;

            if (String.IsNullOrWhiteSpace(applicationDialog.Path) || applicationDialog.Path.ContainsInvalidPathChars() || !Directory.Exists(applicationDialog.Path))
            {
                DialogService.ShowMessageDialog(Strings.SelectedPathNotValid);
                return;
            }
            
            ConfigurationService.Configuration.ApplicationFolders.Add(new ApplicationFolder()
            {
                Id = Guid.NewGuid(),
                Name = applicationDialog.Name,
                Path = applicationDialog.Path,
                EnableSilentInstallation = applicationDialog.EnableSilentInstallation,
                DisableReboot = applicationDialog.DisableReboot,
                EnableInstallationLogging = applicationDialog.EnableInstallationLogging
            });
            await ConfigurationService.SaveConfigurationToDiskAsync();

            // todo: async
            viewModel.RefreshApplicationsCommand.Execute(null);
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.ShowErrorDialog(exception);
            viewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
