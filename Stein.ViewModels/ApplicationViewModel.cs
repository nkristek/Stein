using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class ApplicationViewModel
        : ViewModel
    {
        public ViewModelCommand<ApplicationViewModel> EditApplicationCommand { get; set; }
        
        public ViewModelCommand<ApplicationViewModel> DeleteApplicationCommand { get; set; }
        
        public AsyncViewModelCommand<ApplicationViewModel> InstallApplicationCommand { get; set; }
        
        public AsyncViewModelCommand<ApplicationViewModel> UninstallApplicationCommand { get; set; }
        
        public AsyncViewModelCommand<ApplicationViewModel> CustomOperationApplicationCommand { get; set; }
        
        private string _name;

        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private string _path;

        /// <summary>
        /// Path to the application folder
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value, out _);
        }

        private Guid _folderId;

        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
        public Guid FolderId
        {
            get => _folderId;
            set => SetProperty(ref _folderId, value, out _);
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

        private bool _automaticallyDeleteInstallationLogs;

        /// <summary>
        /// If installation logs should be deleted automatically
        /// </summary>
        public bool AutomaticallyDeleteInstallationLogs
        {
            get => _automaticallyDeleteInstallationLogs;
            set => SetProperty(ref _automaticallyDeleteInstallationLogs, value, out _);
        }

        private int _keepNewestInstallationLogs;

        /// <summary>
        /// How many installation logs should be kept. The oldest ones will be deleted first.
        /// </summary>
        public int KeepNewestInstallationLogs
        {
            get => _keepNewestInstallationLogs;
            set => SetProperty(ref _keepNewestInstallationLogs, value, out _);
        }
    }
}
