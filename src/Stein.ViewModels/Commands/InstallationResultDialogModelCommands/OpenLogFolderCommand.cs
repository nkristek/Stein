using System;
using System.IO;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.InstallationResultDialogModelCommands
{
    public sealed class OpenLogFolderCommand
        : ViewModelCommand<InstallationResultDialogModel>
    {
        private readonly IUriService _uriService;

        public OpenLogFolderCommand(IUriService uriService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(InstallationResultDialogModel.LogFolderPath))]
        protected override bool CanExecute(InstallationResultDialogModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.LogFolderPath) && Directory.Exists(viewModel.LogFolderPath);
        }

        /// <inheritdoc />
        protected override void Execute(InstallationResultDialogModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.LogFolderPath);
        }
    }
}
