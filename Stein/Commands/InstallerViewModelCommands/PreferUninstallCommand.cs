using Stein.ViewModels;
﻿using nkristek.MVVMBase.Commands;

namespace Stein.Commands.InstallerViewModelCommands
{
    public class PreferUninstallCommand
        : ViewModelCommand<InstallerViewModel>
    {
        public PreferUninstallCommand(InstallerViewModel parent) : base(parent) { }

        protected override bool CanExecute(InstallerViewModel viewModel, object view, object parameter)
        {
            return viewModel.IsInstalled.HasValue 
                && viewModel.IsInstalled.Value 
                && viewModel.PreferredOperation != InstallerOperationType.Uninstall;
        }

        protected override void ExecuteSync(InstallerViewModel viewModel, object view, object parameter)
        {
            viewModel.PreferredOperation = InstallerOperationType.Uninstall;
        }
    }
}
