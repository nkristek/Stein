using System;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class EditApplicationCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public EditApplicationCommand(ApplicationViewModel parent, IDialogService dialogService, IViewModelService viewModelService) 
            : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
        }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override void DoExecute(ApplicationViewModel viewModel, object parameter)
        {
            try
            {
                var dialogModel = _viewModelService.CreateViewModel<ApplicationDialogModel>(viewModel);
                if (_dialogService.ShowDialog(dialogModel) != true)
                    return;

                _viewModelService.SaveViewModel(dialogModel);
                _viewModelService.UpdateViewModel(viewModel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
            finally
            {
                (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.Execute(null);
            }
        }
    }
}
