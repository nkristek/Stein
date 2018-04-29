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

        private readonly IDialogService _dialogService;

        public OpenLogFolderCommand(ApplicationDialogModel parent, IDialogService dialogService) : base(parent)
        {
            _dialogService = dialogService;
        }
        
        protected override void DoExecute(ApplicationDialogModel viewModel, object parameter)
        {
            var directoryName = GetLogFolderPath(viewModel.Name);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            Process.Start(directoryName);
        }

        private static string GetLogFolderPath(string applicationName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein", "Logs", applicationName);
        }
        protected override void OnThrownException(ApplicationDialogModel viewModel, object parameter, Exception exception)
        {
            Log.Error("Open log folder", exception);
            _dialogService.ShowError(exception);
        }
    }
}
