using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.InstallerViewModelCommands
{
    public class PreferUninstallCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferUninstallCommand(InstallerViewModel parent) : base(parent) { }

        public override bool CanExecute(InstallerViewModel viewModel, object view, object parameter)
        {
            return viewModel.IsInstalled.HasValue 
                && viewModel.IsInstalled.Value 
                && viewModel.PreferredOperation != ConfigurationTypes.InstallerOperationType.Uninstall;
        }

        public override void Execute(InstallerViewModel viewModel, object view, object parameter)
        {
            viewModel.PreferredOperation = ConfigurationTypes.InstallerOperationType.Uninstall;
        }
    }
}
