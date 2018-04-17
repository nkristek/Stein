using nkristek.MVVMBase.Commands;
using System;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class ShowInfoDialogCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public ShowInfoDialogCommand(MainWindowViewModel parent) : base(parent) { }

        protected override void DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
