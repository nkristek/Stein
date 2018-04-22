using System.Diagnostics;
using nkristek.MVVMBase.Commands;

namespace Stein.ViewModels.Commands.AboutDialogModelCommands
{
    public class OpenUriCommand
        : ViewModelCommand<AboutDialogModel>
    {
        public OpenUriCommand(AboutDialogModel parent) : base(parent) { }

        protected override void DoExecute(AboutDialogModel viewModel, object parameter)
        {
            Process.Start(new ProcessStartInfo(viewModel.Uri.AbsoluteUri));
        }
    }
}
