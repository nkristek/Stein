using System;
using System.Diagnostics;
using System.IO;
using log4net;
using nkristek.MVVMBase.Commands;
using Stein.Services;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public class OpenLogFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OpenLogFolderCommand(ApplicationDialogModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationDialogModel viewModel, object parameter)
        {
            return Directory.Exists(GetLogFolderPath());
        }

        protected override void DoExecute(ApplicationDialogModel viewModel, object parameter)
        {
            Process.Start(GetLogFolderPath());
        }

        private static string GetLogFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein", "Logs");
        }
        protected override void OnThrownException(ApplicationDialogModel viewModel, object parameter, Exception exception)
        {
            Log.Error("Open log folder", exception);
            DialogService.Instance.ShowError(exception);
        }
    }
}
