using nkristek.MVVMBase.Commands;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.InstallerViewModelCommands
{
    public class PreferNothingCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferNothingCommand(InstallerViewModel parent) : base(parent) { }

        protected override bool CanExecute(InstallerViewModel viewModel, object view, object parameter)
        {
            return viewModel.PreferredOperation != InstallerOperationType.DoNothing;
        }

        protected override void ExecuteSync(InstallerViewModel viewModel, object view, object parameter)
        {
            viewModel.PreferredOperation = InstallerOperationType.DoNothing;
        }
    }
}
