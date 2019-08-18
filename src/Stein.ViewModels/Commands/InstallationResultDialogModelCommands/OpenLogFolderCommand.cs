using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.Common.IOService;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.InstallationResultDialogModelCommands
{
    public sealed class OpenLogFolderCommand
        : ViewModelCommand<InstallationResultDialogModel>
    {
        private readonly IUriService _uriService;

        private readonly IIOService _ioService;

        public OpenLogFolderCommand(IUriService uriService, IIOService ioService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            _ioService = ioService ?? throw new ArgumentNullException(nameof(ioService));
        }

        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(InstallationResultDialogModel.LogFolderPath)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(InstallationResultDialogModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.LogFolderPath) && _ioService.DirectoryExists(viewModel.LogFolderPath);
        }

        /// <inheritdoc />
        protected override void Execute(InstallationResultDialogModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.LogFolderPath);
        }
    }
}
