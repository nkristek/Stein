using nkristek.MVVMBase.Commands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.InstallerViewModelCommands
{
    public class PreferNothingCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferNothingCommand(InstallerViewModel parent) : base(parent) { }

        protected override bool CanExecute(InstallerViewModel viewModel, object parameter)
        {
            return viewModel.PreferredOperation != InstallerOperationType.DoNothing;
        }

        protected override void DoExecute(InstallerViewModel viewModel,  object parameter)
        {
            viewModel.PreferredOperation = InstallerOperationType.DoNothing;
        }
    }
}
