using System;
using System.Diagnostics;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.AboutDialogModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<AboutDialogModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public OpenUriCommand(AboutDialogModel parent, IDialogService dialogService)
            : base(parent)
        {
            _dialogService = dialogService;
        }

        protected override void DoExecute(AboutDialogModel viewModel, object parameter)
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
