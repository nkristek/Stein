using System;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using Stein.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class EditApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public EditApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override async Task DoExecute(ApplicationViewModel viewModel, object parameter)
        {
            var dialogModel = ViewModelService.Instance.CreateViewModel<ApplicationDialogModel>(viewModel);
            if (DialogService.Instance.ShowDialog(dialogModel) != true)
                return;
            
            ViewModelService.Instance.SaveViewModel(dialogModel);
            ViewModelService.Instance.UpdateViewModel(viewModel);
            await (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.ExecuteAsync(null);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.Instance.ShowError(exception);
            (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.Execute(null);
        }
    }
}
