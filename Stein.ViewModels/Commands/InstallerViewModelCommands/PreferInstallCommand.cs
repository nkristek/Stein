using nkristek.MVVMBase.Commands;
using Stein.ViewModels.Types;
using System;

namespace Stein.ViewModels.Commands.InstallerViewModelCommands
{
    public class PreferInstallCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferInstallCommand(InstallerViewModel parent) : base(parent) { }

        protected override bool CanExecute(InstallerViewModel viewModel, object parameter)
        {
            return viewModel.IsInstalled.HasValue 
                && !viewModel.IsInstalled.Value 
                && viewModel.PreferredOperation != InstallerOperationType.Install;
        }

        protected override void DoExecute(InstallerViewModel viewModel, object parameter)
        {
            viewModel.PreferredOperation = InstallerOperationType.Install;
        }
    }
}
