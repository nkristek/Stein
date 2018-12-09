using System;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class ShowInfoDialogCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public ShowInfoDialogCommand(MainWindowViewModel parent, IDialogService dialogService, IViewModelService viewModelService) 
            : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
        }
        
        protected override void Execute(MainWindowViewModel viewModel, object parameter)
        {
            try
            {
                var dialogModel = _viewModelService.CreateViewModel<AboutDialogModel>(viewModel);
                _dialogService.ShowDialog(dialogModel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
