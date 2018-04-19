using System;
using System.Threading.Tasks;
using log4net;
using nkristek.MVVMBase.Commands;
using Stein.Services;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class DeleteApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DeleteApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return viewModel.Parent is MainWindowViewModel parent && parent.CurrentInstallation == null;
        }

        protected override async Task DoExecute(ApplicationViewModel viewModel, object parameter)
        {
            ViewModelService.Instance.DeleteViewModel(viewModel);

            await (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.ExecuteAsync(null);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object parameter, Exception exception)
        {
            Log.Error(exception);
            DialogService.Instance.ShowError(exception);
            (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.Execute(null);
        }
    }
}
