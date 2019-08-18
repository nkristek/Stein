using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.Common.IOService;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.ApplicationDialogModelCommands
{
    public sealed class OpenLogFolderCommand
        : ViewModelCommand<ApplicationDialogModel>
    {
        private readonly IUriService _uriService;

        private readonly IIOService _ioService;

        private readonly string _logFolderPath;

        public OpenLogFolderCommand(IUriService uriService, IIOService ioService, string logFolderPath)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            _ioService = ioService ?? throw new ArgumentNullException(nameof(ioService));
            _logFolderPath = !String.IsNullOrEmpty(logFolderPath) ? logFolderPath : throw new ArgumentNullException(nameof(logFolderPath));
        }

        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null 
                || String.IsNullOrEmpty(e.PropertyName) 
                || e.PropertyName.Equals(nameof(ApplicationDialogModel.Name)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(ApplicationDialogModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.Name);
        }

        /// <inheritdoc />
        protected override void Execute(ApplicationDialogModel viewModel, object parameter)
        {
            var directoryName = GetLogFolderPath(viewModel.Name);
            if (!_ioService.DirectoryExists(directoryName))
                _ioService.CreateDirectory(directoryName);
            _uriService.OpenUri(directoryName);
        }

        private string GetLogFolderPath(string applicationName)
        {
            return _ioService.PathCombine(
                _logFolderPath, 
                applicationName);
        }
    }
}
