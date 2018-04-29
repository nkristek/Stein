using nkristek.MVVMBase.Commands;
using Stein.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class ShowInfoDialogCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public ShowInfoDialogCommand(MainWindowViewModel parent, IDialogService dialogService, IViewModelService viewModelService) : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
        }
        
        protected override void DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            var dialogModel = _viewModelService.CreateViewModel<AboutDialogModel>(viewModel);
            _dialogService.ShowDialog(dialogModel);
        }
    }
}
