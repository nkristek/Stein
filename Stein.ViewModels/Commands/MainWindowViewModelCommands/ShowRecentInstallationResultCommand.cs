using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class ShowRecentInstallationResultCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        private readonly IDialogService _dialogService;

        public ShowRecentInstallationResultCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(MainWindowViewModel.RecentInstallationResult))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.RecentInstallationResult != null;
        }

        /// <inheritdoc />
        protected override void Execute(MainWindowViewModel viewModel, object parameter)
        {
            _dialogService.ShowDialog(viewModel.RecentInstallationResult);
        }
    }
}
