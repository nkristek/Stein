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

namespace Stein.Commands.MainViewModelCommands
{
    public class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainViewModel>
    {
        public RefreshApplicationsCommand(MainViewModel parent) : base(parent) { }

        public override bool CanExecute(MainViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainViewModel viewModel, object view, object parameter)
        {
            viewModel.Applications.Clear();
            
            var applications = await Task.Run(() =>
            {
                AppConfigurationService.ReloadAppConfiguration();
                InstallService.RefreshInstalledPrograms();
                return ViewModelService.CreateApplicationViewModels(viewModel).ToList();
            });
            
            foreach (var application in applications)
                viewModel.Applications.Add(application);

            CommandManager.InvalidateRequerySuggested();
        }
    }
}
