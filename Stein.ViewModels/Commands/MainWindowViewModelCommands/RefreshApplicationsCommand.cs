using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.Localizations;
using Stein.Presentation;
using Stein.Services.Extensions;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        private readonly IConfigurationService _configurationService;

        private readonly IInstallService _installService;

        private readonly IMsiService _msiService;

        public RefreshApplicationsCommand(MainWindowViewModel parent, IDialogService dialogService, IViewModelService viewModelService, IConfigurationService configurationService, IInstallService installService, IMsiService msiService) : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
            _configurationService = configurationService;
            _installService = installService;
            _msiService = msiService;
        }

        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task DoExecute(MainWindowViewModel viewModel, object parameter)
        {
            // save changes from application viewmodels back to the configuration
            foreach (var changedApplication in viewModel.Applications.Where(application => application.IsDirty))
                _viewModelService.SaveViewModel(changedApplication);

            // get new installers
            foreach (var applicationFolder in _configurationService.Configuration.ApplicationFolders)
            {
                try
                {
                    await applicationFolder.SyncWithDiskAsync(_msiService);
                }
                catch (Exception exception)
                {
                    applicationFolder.SubFolders.Clear();
                    _dialogService.ShowMessage(String.Format(Strings.RefreshFailed, applicationFolder.Path, exception.Message));
                }
            }
            await _configurationService.SaveConfigurationAsync();
            await Task.Run(() => _installService.RefreshInstalledPrograms());

            // update the viewmodels
            var applications = _viewModelService.CreateViewModels(viewModel, viewModel.Applications.ToList());
            viewModel.Applications.Clear();
            foreach (var application in applications)
                viewModel.Applications.Add(application);

            viewModel.IsDirty = false;
        }

        protected override void OnThrownException(MainWindowViewModel viewModel, object parameter, Exception exception)
        {
            Log.Error(exception);
            _dialogService.ShowError(exception);
        }
    }
}
