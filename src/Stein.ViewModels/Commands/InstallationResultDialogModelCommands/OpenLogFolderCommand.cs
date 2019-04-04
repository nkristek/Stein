using System;
using System.Diagnostics;
using System.IO;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.InstallationResultDialogModelCommands
{
    public sealed class OpenLogFolderCommand
        : ViewModelCommand<InstallationResultDialogModel>
    {
        /// <inheritdoc />
        [CanExecuteSource(nameof(InstallationResultDialogModel.LogFolderPath))]
        protected override bool CanExecute(InstallationResultDialogModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.LogFolderPath) && Directory.Exists(viewModel.LogFolderPath);
        }

        /// <inheritdoc />
        protected override void Execute(InstallationResultDialogModel viewModel, object parameter)
        {
            Process.Start(viewModel.LogFolderPath);
        }
    }
}
