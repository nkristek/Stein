using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class ApplicationViewModel
        : ViewModel
    {
        private Guid _entityId;
        
        public Guid EntityId
        {
            get => _entityId;
            set => SetProperty(ref _entityId, value, out _);
        }

        private string _name;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }
        
        private bool _enableSilentInstallation;
        
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value, out _);
        }

        private bool _disableReboot;
        
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value, out _);
        }

        private bool _enableInstallationLogging;
        
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value, out _);
        }

        private bool _automaticallyDeleteInstallationLogs;
        
        public bool AutomaticallyDeleteInstallationLogs
        {
            get => _automaticallyDeleteInstallationLogs;
            set => SetProperty(ref _automaticallyDeleteInstallationLogs, value, out _);
        }

        private string _keepNewestInstallationLogsString;
        
        public string KeepNewestInstallationLogsString
        {
            get => _keepNewestInstallationLogsString;
            set => SetProperty(ref _keepNewestInstallationLogsString, value, out _);
        }
        
        public int KeepNewestInstallationLogs
        {
            get => int.TryParse(KeepNewestInstallationLogsString, out var value) ? value : 0;
            set => KeepNewestInstallationLogsString = value.ToString();
        }

        private bool _filterDuplicateInstallers;
        
        public bool FilterDuplicateInstallers
        {
            get => _filterDuplicateInstallers;
            set => SetProperty(ref _filterDuplicateInstallers, value, out _);
        }

        public ObservableCollection<InstallerBundleViewModel> InstallerBundles { get; } = new ObservableCollection<InstallerBundleViewModel>();

        private InstallerBundleViewModel _selectedInstallerBundle;

        public InstallerBundleViewModel SelectedInstallerBundle
        {
            get => _selectedInstallerBundle;
            set => SetProperty(ref _selectedInstallerBundle, value, out _);
        }
    }
}
