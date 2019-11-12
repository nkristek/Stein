using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowDialogModelCommands
{
    public class ShowRecentInstallationResultCommand
        : ViewModelCommand<MainWindowDialogModel>
    {
        private readonly IDialogService _dialogService;

        public ShowRecentInstallationResultCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(MainWindowDialogModel.RecentInstallationResult)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(MainWindowDialogModel? viewModel, object? parameter)
        {
            return viewModel?.RecentInstallationResult != null;
        }

        /// <inheritdoc />
        protected override void Execute(MainWindowDialogModel? viewModel, object? parameter)
        {
            if (viewModel?.RecentInstallationResult is InstallationResultDialogModel result)
                _dialogService.ShowDialog(result);
        }
    }
}
