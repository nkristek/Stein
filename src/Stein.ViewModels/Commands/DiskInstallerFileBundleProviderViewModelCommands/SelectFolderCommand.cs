using System;
using NKristek.Smaragd.Commands;
using Stein.Localization;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.DiskInstallerFileBundleProviderViewModelCommands
{
    public sealed class SelectFolderCommand
        : ViewModelCommand<DiskInstallerFileBundleProviderViewModel>
    {
        private readonly IDialogService _dialogService;

        public SelectFolderCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <inheritdoc />
        protected override void Execute(DiskInstallerFileBundleProviderViewModel viewModel, object parameter)
        {
            if (_dialogService.ShowSelectFolderDialog(out var folderPath, Strings.SelectFolder) == true)
                viewModel.Path = folderPath;
        }
    }
}
