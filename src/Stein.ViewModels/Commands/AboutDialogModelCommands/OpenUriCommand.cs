using System;
using System.Diagnostics;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.AboutDialogModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<AboutDialogModel>
    {
        /// <inheritdoc />
        [CanExecuteSource(nameof(AboutDialogModel.Uri))]
        protected override bool CanExecute(AboutDialogModel viewModel, object parameter)
        {
            return viewModel.Uri != null && !String.IsNullOrEmpty(viewModel.Uri.AbsoluteUri);
        }

        /// <inheritdoc />
        protected override void Execute(AboutDialogModel viewModel, object parameter)
        {
            Process.Start(new ProcessStartInfo(viewModel.Uri.AbsoluteUri));
        }
    }
}
