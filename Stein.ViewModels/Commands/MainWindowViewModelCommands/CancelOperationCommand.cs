using System;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class CancelOperationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public CancelOperationCommand(MainWindowViewModel parent, IDialogService dialogService) 
            : base(parent)
        {
            _dialogService = dialogService;
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation != null 
                && viewModel.CurrentInstallation.State != InstallationState.Preparing 
                && viewModel.CurrentInstallation.State != InstallationState.Cancelled;
        }

        protected override void DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            try
            {
                viewModel.CurrentInstallation.State = InstallationState.Cancelled;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
