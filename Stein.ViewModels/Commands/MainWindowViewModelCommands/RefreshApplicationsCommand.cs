using System;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Localizations;
using Stein.Presentation;
using Stein.Services;
using Stein.Services.Extensions;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class RefreshApplicationsCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        private readonly IConfigurationService _configurationService;

        private readonly IInstallService _installService;

        private readonly IMsiService _msiService;

        public RefreshApplicationsCommand(MainWindowViewModel parent, IDialogService dialogService, IViewModelService viewModelService, IConfigurationService configurationService, IInstallService installService, IMsiService msiService) 
            : base(parent)
        {
            _dialogService = dialogService;
            _viewModelService = viewModelService;
            _configurationService = configurationService;
            _installService = installService;
            _msiService = msiService;
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object parameter)
        {
            try
            {
                _viewModelService.SaveViewModel(viewModel);

                foreach (var applicationFolder in _configurationService.Configuration.ApplicationFolders)
                {
                    try
                    {
                        await applicationFolder.SyncWithDiskAsync(_msiService);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                        applicationFolder.SubFolders.Clear();
                        _dialogService.ShowMessage(String.Format(Strings.RefreshFailed, applicationFolder.Path, exception.Message));
                    }
                }

                await _configurationService.SaveConfigurationAsync();
                await Task.Run(() => _installService.RefreshInstalledPrograms());
                
                _viewModelService.UpdateViewModel(viewModel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
