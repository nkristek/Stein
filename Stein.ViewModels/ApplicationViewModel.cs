using System;
using System.Collections.ObjectModel;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using Stein.ViewModels.Commands.ApplicationViewModelCommands;

namespace Stein.ViewModels
{
    public class ApplicationViewModel
        : ViewModel
    {
        public ApplicationViewModel()
        {
            EditApplicationCommand = new EditApplicationCommand(this);
            DeleteApplicationCommand = new DeleteApplicationCommand(this);
            InstallApplicationCommand = new InstallApplicationCommand(this);
            UninstallApplicationCommand = new UninstallApplicationCommand(this);
            CustomOperationApplicationCommand = new CustomOperationApplicationCommand(this);
        }

        [CommandCanExecuteSource(nameof(Parent))]
        public AsyncViewModelCommand<ApplicationViewModel> EditApplicationCommand { get; }

        [CommandCanExecuteSource(nameof(Parent))]
        public AsyncViewModelCommand<ApplicationViewModel> DeleteApplicationCommand { get; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> InstallApplicationCommand { get; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> UninstallApplicationCommand { get; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> CustomOperationApplicationCommand { get; }
        
        private string _name;
        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _path;
        /// <summary>
        /// Path to the application folder
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        private Guid _folderId;
        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
        public Guid FolderId
        {
            get => _folderId;
            set => SetProperty(ref _folderId, value);
        }

        private bool _enableSilentInstallation;
        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value);
        }

        private bool _disableReboot;
        /// <summary>
        /// If the installers should be able to automatically reboot if necessary
        /// </summary>
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value);
        }

        private bool _enableInstallationLogging;
        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value);
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
            set => SetProperty(ref _selectedInstallerBundle, value);
        }
    }
}
