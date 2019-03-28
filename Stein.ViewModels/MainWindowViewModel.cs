using System.Collections.ObjectModel;
using System.ComponentModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;
using Stein.Presentation;

namespace Stein.ViewModels
{
    public sealed class MainWindowViewModel
        : ViewModel
    {
        private readonly IThemeService _themeService;

        public MainWindowViewModel(IThemeService themeService)
        {
            _themeService = themeService;
            _themeService.ThemeChanged += (sender, args) => RaisePropertyChanged(nameof(CurrentTheme));
        }
        
        /// <summary>
        /// List of all applications
        /// </summary>
        public ObservableCollection<ApplicationViewModel> Applications { get; } = new ObservableCollection<ApplicationViewModel>();
        
        private InstallationViewModel _currentInstallation;

        /// <summary>
        /// Is set if an operation is in progress
        /// </summary>
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
                }
            }
        }

        private void CurrentInstallationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CurrentInstallation));
        }

        /// <summary>
        /// Current UI theme
        /// </summary>
        public Theme CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set => _themeService.SetTheme(value);
        }
        
        private bool _isUpdating;

        // TODO: move to base class
        /// <summary>
        /// If this <see cref="IViewModel"/> is updating.
        /// </summary>
        [IsDirtyIgnored]
        public bool IsUpdating
        {
            get => _isUpdating;
            set => SetProperty(ref _isUpdating, value, out _);
        }
    }
}
