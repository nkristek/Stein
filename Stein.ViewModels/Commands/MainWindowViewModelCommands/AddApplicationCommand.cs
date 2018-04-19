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

        public AddApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            var dialogModel = ViewModelService.Instance.CreateViewModel<ApplicationDialogModel>(viewModel);
            if (DialogService.Instance.ShowDialog(dialogModel) != true)
                return;

            if (String.IsNullOrWhiteSpace(dialogModel.Path) || dialogModel.Path.ContainsInvalidPathChars() || !Directory.Exists(dialogModel.Path))
            {
                DialogService.Instance.ShowMessage(Strings.SelectedPathNotValid);
                return;
            }

            ViewModelService.Instance.SaveViewModel(dialogModel);
            await viewModel.RefreshApplicationsCommand.ExecuteAsync(null);
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object parameter, Exception exception)
        {
            Log.Error(exception);
            DialogService.Instance.ShowError(exception);
            viewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
