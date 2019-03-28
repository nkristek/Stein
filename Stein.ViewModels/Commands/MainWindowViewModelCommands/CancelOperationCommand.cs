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

        public CancelOperationCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation != null && !viewModel.CurrentInstallation.CancellationTokenSource.IsCancellationRequested;
        }

        protected override void Execute(MainWindowViewModel viewModel, object parameter)
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
