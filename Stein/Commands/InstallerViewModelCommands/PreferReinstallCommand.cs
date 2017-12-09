using nkristek.MVVMBase.Commands;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.InstallerViewModelCommands
{
    public class PreferReinstallCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferReinstallCommand(InstallerViewModel parent) : base(parent) { }

        protected override bool CanExecute(InstallerViewModel viewModel, object view, object parameter)
        {
            return viewModel.IsInstalled.HasValue
                && viewModel.IsInstalled.Value
                && viewModel.PreferredOperation != InstallerOperationType.Reinstall;
        }

        protected override void ExecuteSync(InstallerViewModel viewModel, object view, object parameter)
        {
            viewModel.PreferredOperation = InstallerOperationType.Reinstall;
        }
    }
}
