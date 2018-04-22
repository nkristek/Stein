using nkristek.MVVMBase.Commands;
using Stein.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class ShowInfoDialogCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public ShowInfoDialogCommand(MainWindowViewModel parent) : base(parent) { }

        protected override void DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            var dialogModel = ViewModelService.Instance.CreateViewModel<AboutDialogModel>(viewModel);
            DialogService.Instance.ShowDialog(dialogModel);
        }
    }
}
