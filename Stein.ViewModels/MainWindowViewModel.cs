using System.Collections.ObjectModel;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using Stein.Services;
using Stein.Services.Types;
using Stein.ViewModels.Commands.MainWindowViewModelCommands;

namespace Stein.ViewModels
{
    public class MainWindowViewModel
        : ViewModel
    {
        private readonly IThemeService _themeService;

        private readonly IProgressBarService _progressBarService;

        public MainWindowViewModel(IDialogService dialogService, IViewModelService viewModelService, IThemeService themeService, IProgressBarService progressBarService, IConfigurationService configurationService, IInstallService installService, IMsiService msiService)
        {
            _themeService = themeService;
            _themeService.ThemeChanged += (sender, args) => { RaisePropertyChanged(nameof(CurrentTheme)); };
            _progressBarService = progressBarService;

            RefreshApplicationsCommand = new RefreshApplicationsCommand(this, dialogService, viewModelService, configurationService, installService, msiService);
            AddApplicationCommand = new AddApplicationCommand(this, dialogService, viewModelService);
            CancelOperationCommand = new CancelOperationCommand(this);
            ShowInfoDialogCommand = new ShowInfoDialogCommand(this, dialogService, viewModelService);
            ChangeThemeCommand = new ChangeThemeCommand(this);

            RefreshApplicationsCommand.Execute(null);
        }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public ViewModelCommand<MainWindowViewModel> CancelOperationCommand { get; }

        public ViewModelCommand<MainWindowViewModel> ShowInfoDialogCommand { get; }

        public ViewModelCommand<MainWindowViewModel> ChangeThemeCommand { get; }

        /// <summary>
        /// List of all applications
        /// </summary>
        public ObservableCollection<ApplicationViewModel> Applications { get; } = new ObservableCollection<ApplicationViewModel>();
        
        private InstallationViewModel _currentInstallation;
        /// <summary>
        /// Is set if an operation is in progress
        /// </summary>
        public InstallationViewModel CurrentInstallation
        {
            get => _currentInstallation;
            set
            {
                if (SetProperty(ref _currentInstallation, value))
                    _progressBarService.SetState(value != null ? ProgressBarState.Indeterminate : ProgressBarState.None);
            }
        }

        private InstallationResultViewModel _installationResult;
        /// <summary>
        /// Is set if an operation was finished
        /// </summary>
        public InstallationResultViewModel InstallationResult
        {
            get => _installationResult;
            set => SetProperty(ref _installationResult, value);
        }
        
        /// <summary>
        /// Current UI theme
        /// </summary>
        public Theme CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set => _themeService.SetTheme(value);
        }
    }
}
