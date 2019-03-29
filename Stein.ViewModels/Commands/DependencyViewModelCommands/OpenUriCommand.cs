using System.Diagnostics;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.DependencyViewModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<DependencyViewModel>
    {
        protected override void Execute(DependencyViewModel viewModel, object parameter)
        {
            Process.Start(new ProcessStartInfo(viewModel.Uri.AbsoluteUri));
        }
    }
}
