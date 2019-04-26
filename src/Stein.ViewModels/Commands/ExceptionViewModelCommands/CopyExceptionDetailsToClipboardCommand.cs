using System;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.ExceptionViewModelCommands
{
    public class CopyExceptionDetailsToClipboardCommand
        : ViewModelCommand<ExceptionViewModel>
    {
        private readonly IClipboardService _clipboardService;

        public CopyExceptionDetailsToClipboardCommand(IClipboardService clipboardService)
        {
            _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        }

        /// <inheritdoc />
        protected override void Execute(ExceptionViewModel viewModel, object parameter)
        {
            _clipboardService.SetText(viewModel.ExceptionText);
        }
    }
}
