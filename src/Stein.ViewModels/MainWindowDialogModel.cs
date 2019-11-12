using System.Collections.ObjectModel;
using System.ComponentModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;
using Stein.Presentation;

namespace Stein.ViewModels
{
    public sealed class MainWindowDialogModel
        : DialogModel
    {
        private readonly IThemeService _themeService;

        public MainWindowDialogModel(IThemeService themeService)
        {
            _themeService = themeService;
            _themeService.ThemeChanged += (sender, args) => NotifyPropertyChanged(nameof(CurrentTheme));
        }

        private InstallationViewModel? _currentInstallation;
        
        [IsDirtyIgnored]
        public InstallationViewModel? CurrentInstallation
        {
            get => _currentInstallation;
            set
            {
                if (SetProperty(ref _currentInstallation, value, out var oldValue))
                {
                    if (oldValue != null)
                    {
                        oldValue.PropertyChanging -= CurrentInstallationOnPropertyChanging;
                        oldValue.PropertyChanged -= CurrentInstallationOnPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanging += CurrentInstallationOnPropertyChanging;
                        value.PropertyChanged += CurrentInstallationOnPropertyChanged;
                    }
                }
            }
        }

        private void CurrentInstallationOnPropertyChanging(object? sender, PropertyChangingEventArgs? e)
        {
            NotifyPropertyChanging(nameof(CurrentInstallation));
        }

        private void CurrentInstallationOnPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            NotifyPropertyChanged(nameof(CurrentInstallation));
        }
        
        public Theme CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set => _themeService.SetTheme(value);
        }
        
        private InstallationResultDialogModel? _recentInstallationResult;
        
        [IsDirtyIgnored]
        public InstallationResultDialogModel? RecentInstallationResult
        {
            get => _recentInstallationResult;
            set => SetProperty(ref _recentInstallationResult, value);
        }

        private ObservableCollection<ApplicationViewModel> _application = new ObservableCollection<ApplicationViewModel>();

        public ObservableCollection<ApplicationViewModel> Applications
        {
            get => _application;
            set => SetProperty(ref _application, value);
        }

        private UpdateDialogModel? _availableUpdate;

        public UpdateDialogModel? AvailableUpdate
        {
            get => _availableUpdate;
            set => SetProperty(ref _availableUpdate, value);
        }

        private IViewModelCommand<MainWindowDialogModel>? _refreshApplicationsCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<MainWindowDialogModel>? RefreshApplicationsCommand
        {
            get => _refreshApplicationsCommand;
            set
            {
                if (SetProperty(ref _refreshApplicationsCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<MainWindowDialogModel>? _addApplicationCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<MainWindowDialogModel>? AddApplicationCommand
        {
            get => _addApplicationCommand;
            set
            {
                if (SetProperty(ref _addApplicationCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<MainWindowDialogModel>? _showInfoDialogCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<MainWindowDialogModel>? ShowInfoDialogCommand
        {
            get => _showInfoDialogCommand;
            set
            {
                if (SetProperty(ref _showInfoDialogCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<MainWindowDialogModel>? _changeThemeCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<MainWindowDialogModel>? ChangeThemeCommand
        {
            get => _changeThemeCommand;
            set
            {
                if (SetProperty(ref _changeThemeCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<MainWindowDialogModel>? _showRecentInstallationResultCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<MainWindowDialogModel>? ShowRecentInstallationResultCommand
        {
            get => _showRecentInstallationResultCommand;
            set
            {
                if (SetProperty(ref _showRecentInstallationResultCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<MainWindowDialogModel>? _showUpdateDialogCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<MainWindowDialogModel>? ShowUpdateDialogCommand
        {
            get => _showUpdateDialogCommand;
            set
            {
                if (SetProperty(ref _showUpdateDialogCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }
    }
}
