using System;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Localizations;
using Stein.Presentation;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public AddApplicationCommand(IDialogService dialogService, IViewModelService viewModelService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation), nameof(MainWindowViewModel.IsUpdating))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null && !viewModel.IsUpdating;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object parameter)
        {
            try
            {
                var dialogModel = _viewModelService.CreateViewModel<ApplicationDialogModel>(viewModel);
                do
                {
                    if (_dialogService.ShowDialog(dialogModel) != true)
                        return;

                    if (!dialogModel.IsValid)
                        _dialogService.ShowMessage(Strings.DialogInputNotValid);
                } while (!dialogModel.IsValid);

                await _viewModelService.SaveViewModelAsync(dialogModel);
                await _viewModelService.UpdateViewModelAsync(viewModel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
                await _viewModelService.UpdateViewModelAsync(viewModel);
            }
        }
    }
}
