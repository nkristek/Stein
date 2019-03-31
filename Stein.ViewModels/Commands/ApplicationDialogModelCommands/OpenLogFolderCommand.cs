using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public sealed class OpenLogFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        /// <inheritdoc />
        [CanExecuteSource(nameof(ApplicationDialogModel.Name))]
        protected override bool CanExecute(ApplicationDialogModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.Name);
        }

        /// <inheritdoc />
        protected override void Execute(ApplicationDialogModel viewModel, object parameter)
        {
            var directoryName = GetLogFolderPath(viewModel.Name);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            Process.Start(directoryName);
        }

        private static string GetLogFolderPath(string applicationName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetEntryAssembly().GetName().Name, "Logs", applicationName);
        }
    }
}
