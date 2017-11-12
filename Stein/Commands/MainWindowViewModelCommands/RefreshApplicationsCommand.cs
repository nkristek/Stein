using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using System.Windows.Input;
using System.Windows;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public RefreshApplicationsCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            viewModel.Applications.Clear();

            await ConfigurationService.LoadConfigurationFromDiskAsync();

            foreach (var applicationFolder in ConfigurationService.Configuration.ApplicationFolders)
                await ConfigurationService.SyncApplicationFolderWithDiskAsync(applicationFolder);

            await ConfigurationService.SaveConfigurationToDiskAsync();

            await InstallService.RefreshInstalledProgramsAsync();
            
            foreach (var application in ViewModelService.CreateApplicationViewModels(viewModel))
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
        }

        public override void OnThrownExeption(MainWindowViewModel viewModel, object view, object parameter, Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}
