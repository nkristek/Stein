using System;
using System.Diagnostics;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.DependencyViewModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<DependencyViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public OpenUriCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        protected override void Execute(DependencyViewModel viewModel, object parameter)
        {
            try
            {
                Process.Start(new ProcessStartInfo(viewModel.Uri.AbsoluteUri));
            }
            catch (Exception exception)
            {
                Log.Error("Open application uri", exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
