using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.InstallerViewModelCommands
{
    public class PreferNothingCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferNothingCommand(InstallerViewModel parent) : base(parent) { }

        public override bool CanExecute(InstallerViewModel viewModel, object view, object parameter)
        {
            return viewModel.PreferredOperation != ConfigurationTypes.InstallerOperationType.DoNothing;
        }

        public override void Execute(InstallerViewModel viewModel, object view, object parameter)
        {
            viewModel.PreferredOperation = ConfigurationTypes.InstallerOperationType.DoNothing;
        }
    }
}
