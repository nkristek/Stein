using System;
using System.Diagnostics;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.DependencyViewModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<DependencyViewModel>
    {
        /// <inheritdoc />
        [CanExecuteSource(nameof(DependencyViewModel.Uri))]
        protected override bool CanExecute(DependencyViewModel viewModel, object parameter)
        {
            return viewModel.Uri != null && !String.IsNullOrEmpty(viewModel.Uri.AbsoluteUri);
        }

        /// <inheritdoc />
        protected override void Execute(DependencyViewModel viewModel, object parameter)
        {
            Process.Start(new ProcessStartInfo(viewModel.Uri.AbsoluteUri));
        }
    }
}
