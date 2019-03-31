using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class OpenProviderLinkCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        /// <inheritdoc />
        [CanExecuteSource(nameof(ApplicationViewModel.ProviderLink), nameof(ApplicationViewModel.IsUpdating))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.ProviderLink) && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override void Execute(ApplicationViewModel viewModel, object parameter)
        {
            Process.Start(new ProcessStartInfo(viewModel.ProviderLink));
        }
    }
}
