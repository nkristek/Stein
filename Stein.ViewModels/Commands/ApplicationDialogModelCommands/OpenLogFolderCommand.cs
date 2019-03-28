using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public sealed class OpenLogFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public OpenLogFolderCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }
        
        protected override void Execute(ApplicationDialogModel viewModel, object parameter)
        {
            try
            {
                var directoryName = GetLogFolderPath(viewModel.Name);
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
                Process.Start(directoryName);
            }
            catch (Exception exception)
            {
                Log.Error("Open log folder", exception);
                _dialogService.ShowError(exception);
            }
        }

        private static string GetLogFolderPath(string applicationName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetEntryAssembly().GetName().Name, "Logs", applicationName);
        }
    }
}
