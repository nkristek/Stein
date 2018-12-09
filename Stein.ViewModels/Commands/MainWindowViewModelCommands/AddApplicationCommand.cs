using System;
using System.IO;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Helpers;
using Stein.Localizations;
using Stein.Presentation;
using Stein.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class AddApplicationCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public AddApplicationCommand(MainWindowViewModel parent, IDialogService dialogService, IViewModelService viewModelService) 
            : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override void Execute(MainWindowViewModel viewModel, object parameter)
        {
            try
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
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
            finally
            {
                viewModel.RefreshApplicationsCommand.ExecuteAsync(null);
            }
        }
    }
}
