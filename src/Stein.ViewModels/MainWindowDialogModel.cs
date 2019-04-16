using System.Collections.ObjectModel;
using System.ComponentModel;
using NKristek.Smaragd.Attributes;
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
            _themeService.ThemeChanged += (sender, args) => RaisePropertyChanged(nameof(CurrentTheme));
        }

        private InstallationViewModel _currentInstallation;
        
        [IsDirtyIgnored]
        public InstallationViewModel CurrentInstallation
        {
            get => _currentInstallation;
            set
            {
                if (SetProperty(ref _currentInstallation, value, out var oldValue))
                { 
                    if (oldValue != null)
                        oldValue.PropertyChanged -= CurrentInstallationOnPropertyChanged;
                    if (value != null)
                        value.PropertyChanged += CurrentInstallationOnPropertyChanged;
                    foreach (var applicationViewModel in Applications)
                        applicationViewModel.RaisePropertyChanged(nameof(Parent));
                }
            }
        }

        private void CurrentInstallationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CurrentInstallation));
        }
        
        public Theme CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set => _themeService.SetTheme(value);
        }
        
        private InstallationResultDialogModel _recentInstallationResult;
        
        [IsDirtyIgnored]
        public InstallationResultDialogModel RecentInstallationResult
        {
            get => _recentInstallationResult;
            set => SetProperty(ref _recentInstallationResult, value, out _);
        }

        private ObservableCollection<ApplicationViewModel> _application = new ObservableCollection<ApplicationViewModel>();

        public ObservableCollection<ApplicationViewModel> Applications
        {
            get => _application;
            set => SetProperty(ref _application, value, out _);
        }

        private UpdateDialogModel _availableUpdate;

        public UpdateDialogModel AvailableUpdate
        {
            get => _availableUpdate;
            set => SetProperty(ref _availableUpdate, value, out _);
        }
    }
}
