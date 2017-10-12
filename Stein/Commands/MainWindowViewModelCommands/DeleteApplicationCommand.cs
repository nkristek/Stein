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
        : ViewModelCommand<MainWindowViewModel>
    {
        public DeleteApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override void Execute(MainWindowViewModel viewModel, object view, object parameter)
        {
            var applicationToDelete = parameter as ApplicationViewModel;
            if (applicationToDelete == null)
                return;

            var setupToDelete = AppConfigurationService.CurrentConfiguration.Setups.FirstOrDefault(s => s.Path == applicationToDelete.Path);
            if (setupToDelete == null)
                return;

            AppConfigurationService.CurrentConfiguration.Setups.Remove(setupToDelete);
            if (!AppConfigurationService.SaveConfiguration())
                return;

            viewModel.Applications.Remove(applicationToDelete);
        }
    }
}
