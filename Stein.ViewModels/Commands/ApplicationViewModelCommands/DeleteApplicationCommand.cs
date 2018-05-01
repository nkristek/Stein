using System;
using System.Threading.Tasks;
using log4net;
using nkristek.MVVMBase.Commands;
using Stein.Presentation;
using Stein.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class DeleteApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public DeleteApplicationCommand(ApplicationViewModel parent, IDialogService dialogService, IViewModelService viewModelService) : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
        }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override async Task DoExecute(ApplicationViewModel viewModel, object parameter)
        {
            _viewModelService.DeleteViewModel(viewModel);

            await (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.ExecuteAsync(null);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object parameter, Exception exception)
        {
            Log.Error(exception);
            _dialogService.ShowError(exception);
            (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.Execute(null);
        }
    }
}
