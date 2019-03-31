using System.Windows;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.ExceptionViewModelCommands
{
    public class CopyExceptionDetailsToClipboardCommand
        : ViewModelCommand<ExceptionViewModel>
    {
        /// <inheritdoc />
        protected override void Execute(ExceptionViewModel viewModel, object parameter)
        {
            Clipboard.SetText(viewModel.ExceptionText, TextDataFormat.UnicodeText);
        }
    }
}
