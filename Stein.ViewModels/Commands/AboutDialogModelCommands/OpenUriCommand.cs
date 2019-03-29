using System.Diagnostics;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.AboutDialogModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<AboutDialogModel>
    {
        protected override void Execute(AboutDialogModel viewModel, object parameter)
        {
            Process.Start(new ProcessStartInfo(viewModel.Uri.AbsoluteUri));
        }
    }
}
