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
        public AsyncViewModelCommand<ApplicationViewModel> EditApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(Parent))]
        public AsyncViewModelCommand<ApplicationViewModel> DeleteApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> InstallApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> UninstallApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> CustomOperationApplicationCommand { get; private set; }
        
        private string _Name;
        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Path;
        /// <summary>
        /// Path to the application folder
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { SetProperty(ref _Path, value); }
        }

        private Guid _FolderId;
        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
        public Guid FolderId
        {
            get { return _FolderId; }
            set { SetProperty(ref _FolderId, value); }
        }

        private bool _EnableSilentInstallation;
        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
        public bool EnableSilentInstallation
        {
            get { return _EnableSilentInstallation; }
            set { SetProperty(ref _EnableSilentInstallation, value); }
        }

        private bool _DisableReboot;
        /// <summary>
        /// If the installers should be able to automatically reboot if necessary
        /// </summary>
        public bool DisableReboot
        {
            get { return _DisableReboot; }
            set { SetProperty(ref _DisableReboot, value); }
        }

        private bool _EnableInstallationLogging;
        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get { return _EnableInstallationLogging; }
            set { SetProperty(ref _EnableInstallationLogging, value); }
        }

        /// <summary>
        /// List of all installer bundles of this application
        /// </summary>
        public ObservableCollection<InstallerBundleViewModel> InstallerBundles { get; } = new ObservableCollection<InstallerBundleViewModel>();
        
        private InstallerBundleViewModel _SelectedInstallerBundle;
        /// <summary>
        /// The currently selected InstallerBundleViewModel
        /// </summary>
        public InstallerBundleViewModel SelectedInstallerBundle
        {
            get { return _SelectedInstallerBundle; }
            set { SetProperty(ref _SelectedInstallerBundle, value); }
        }
    }
}
