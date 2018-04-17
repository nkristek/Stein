using nkristek.MVVMBase.Commands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.InstallerViewModelCommands
{
    public class PreferReinstallCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferReinstallCommand(InstallerViewModel parent) : base(parent) { }

        protected override bool CanExecute(InstallerViewModel viewModel, object parameter)
        {
            return viewModel.IsInstalled.HasValue
                && viewModel.IsInstalled.Value
                && viewModel.PreferredOperation != InstallerOperationType.Reinstall;
        }

        protected override void DoExecute(InstallerViewModel viewModel, object parameter)
        {
            viewModel.PreferredOperation = InstallerOperationType.Reinstall;
        }
    }
}
