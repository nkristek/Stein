using System;
using System.IO;
using System.Threading.Tasks;
using log4net;
using nkristek.MVVMBase.Commands;
using Stein.Localizations;
using Stein.Services;
using Stein.Helpers;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public AddApplicationCommand(MainWindowViewModel parent, IDialogService dialogService, IViewModelService viewModelService) : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
        }

        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            var dialogModel = _viewModelService.CreateViewModel<ApplicationDialogModel>(viewModel);
            if (_dialogService.ShowDialog(dialogModel) != true)
                return;

            if (String.IsNullOrWhiteSpace(dialogModel.Path) || dialogModel.Path.ContainsInvalidPathChars() || !Directory.Exists(dialogModel.Path))
            {
                _dialogService.ShowMessage(Strings.SelectedPathNotValid);
                return;
            }

            _viewModelService.SaveViewModel(dialogModel);
            await viewModel.RefreshApplicationsCommand.ExecuteAsync(null);
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object parameter, Exception exception)
        {
            Log.Error(exception);
            _dialogService.ShowError(exception);
            viewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
