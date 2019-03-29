using System;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        private readonly IViewModelService _viewModelService;

        public RefreshApplicationsCommand(IViewModelService viewModelService) 
        {
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object parameter)
        {
            await _viewModelService.UpdateViewModelAsync(viewModel);
        }
    }
}
