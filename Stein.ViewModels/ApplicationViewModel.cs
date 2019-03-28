using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class ApplicationViewModel
        : ViewModel
    {
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

        private Guid _entityId;

        /// <summary>
        /// The Id of the associated entity.
        /// </summary>
        public Guid EntityId
        {
            get => _entityId;
            set => SetProperty(ref _entityId, value, out _);
        }

        private string _name;

        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }
        
        /// <summary>
        /// List of all installer bundles of this application
        /// </summary>
        public ObservableCollection<InstallerBundleViewModel> InstallerBundles { get; } = new ObservableCollection<InstallerBundleViewModel>();
        
        private InstallerBundleViewModel _selectedInstallerBundle;

        /// <summary>
        /// The currently selected InstallerBundleViewModel
        /// </summary>
        public InstallerBundleViewModel SelectedInstallerBundle
        {
            get => _selectedInstallerBundle;
            set => SetProperty(ref _selectedInstallerBundle, value, out _);
        }

        private bool _enableSilentInstallation;

        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value, out _);
        }

        private bool _disableReboot;

        /// <summary>
        /// If the installers should be able to automatically reboot if necessary
        /// </summary>
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value, out _);
        }

        private bool _enableInstallationLogging;

        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value, out _);
        }

        private bool _automaticallyDeleteInstallationLogs;

        /// <summary>
        /// If installation logs should be deleted automatically
        /// </summary>
        public bool AutomaticallyDeleteInstallationLogs
        {
            get => _automaticallyDeleteInstallationLogs;
            set => SetProperty(ref _automaticallyDeleteInstallationLogs, value, out _);
        }

        private string _keepNewestInstallationLogsString;

        /// <summary>
        /// How many installation logs should be kept. The oldest ones will be deleted first.
        /// </summary>
        public string KeepNewestInstallationLogsString
        {
            get => _keepNewestInstallationLogsString;
            set => SetProperty(ref _keepNewestInstallationLogsString, value, out _);
        }

        /// <summary>
        /// How many installation logs should be kept. The oldest ones will be deleted first.
        /// </summary>
        public int KeepNewestInstallationLogs
        {
            get => int.TryParse(KeepNewestInstallationLogsString, out var value) ? value : 0;
            set => KeepNewestInstallationLogsString = value.ToString();
        }
    }
}
