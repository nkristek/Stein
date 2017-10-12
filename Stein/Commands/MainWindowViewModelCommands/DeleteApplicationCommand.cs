using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.Configuration;
using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.MainWindowViewModelCommands
{
    public class DeleteApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public DeleteApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            var applicationToDelete = parameter as ApplicationViewModel;
            if (applicationToDelete == null)
                return;

            var setupToDelete = AppConfigurationService.CurrentConfiguration.Setups.FirstOrDefault(s => s.Path == applicationToDelete.Path);
            if (setupToDelete == null)
                return;

            var success = await Task.Run(() =>
            {
                AppConfigurationService.CurrentConfiguration.Setups.Remove(setupToDelete);
                return AppConfigurationService.SaveConfiguration();
            });

            if (success)
                viewModel.Applications.Remove(applicationToDelete);
            else
                await viewModel.RefreshApplicationsCommand.ExecuteAsync(null);
        }
    }
}
