using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;
using Stein.Presentation;

namespace Stein.ViewModels
{
    public sealed class MainWindowViewModel
        : ViewModel
    {
        private readonly IThemeService _themeService;
        
        private readonly IProgressBarService _progressBarService;

        public MainWindowViewModel(IThemeService themeService, IProgressBarService progressBarService)
        {
            _themeService = themeService;
            _themeService.ThemeChanged += (sender, args) => { RaisePropertyChanged(nameof(CurrentTheme)); };
            _progressBarService = progressBarService;
        }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public ViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public ViewModelCommand<MainWindowViewModel> CancelOperationCommand { get; set; }

        public ViewModelCommand<MainWindowViewModel> ShowInfoDialogCommand { get; set; }

        public ViewModelCommand<MainWindowViewModel> ChangeThemeCommand { get; set; }

        /// <summary>
        /// List of all applications
        /// </summary>
        [ChildViewModelCollection]
        public ObservableCollection<ApplicationViewModel> Applications { get; } = new ObservableCollection<ApplicationViewModel>();
        
        private InstallationViewModel _currentInstallation;

        /// <summary>
        /// Is set if an operation is in progress
        /// </summary>
        [IsDirtyIgnored]
        [ChildViewModel]
        public InstallationViewModel CurrentInstallation
        {
            get => _currentInstallation;
            set
            {
                if (SetProperty(ref _currentInstallation, value, out _))
                    _progressBarService.SetState(value != null ? ProgressBarState.Indeterminate : ProgressBarState.None);
            }
        }

        private InstallationResultViewModel _installationResult;

        /// <summary>
        /// Is set if an operation was finished
        /// </summary>
        [IsDirtyIgnored]
        [ChildViewModel]
        public InstallationResultViewModel InstallationResult
        {
            get => _installationResult;
            set => SetProperty(ref _installationResult, value, out _);
        }
        
        /// <summary>
        /// Current UI theme
        /// </summary>
        [IsDirtyIgnored]
        public Theme CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set => _themeService.SetTheme(value);
        }
    }
}
