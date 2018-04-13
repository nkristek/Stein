using nkristek.MVVMBase.Commands;
using nkristek.Stein.ViewModels;
using System;
using System.Threading.Tasks;

namespace nkristek.Stein.Commands.MainWindowViewModelCommands
{
    public class ShowInfoDialogCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public ShowInfoDialogCommand(MainWindowViewModel parent) : base(parent) { }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
