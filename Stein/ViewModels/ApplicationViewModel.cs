using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.Commands.ApplicationViewModelCommands;
using Stein.Configuration;
using WpfBase.Commands;
using WpfBase.ViewModels;
using System.Windows.Input;

namespace Stein.ViewModels
{
    public class ApplicationViewModel
        : ViewModel
    {
        public ApplicationViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            InstallApplicationCommand = new InstallApplicationCommand(this);
            UninstallApplicationCommand = new UninstallApplicationCommand(this);
            SelectFolderCommand = new SelectFolderCommand(this);
            EditApplicationCommand = new EditApplicationCommand(this);

            InstallerBundles.CollectionChanged += InstallerBundles_CollectionChanged;
        }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> InstallApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> UninstallApplicationCommand { get; private set; }
        
        public ViewModelCommand<ApplicationViewModel> SelectFolderCommand { get; private set; }

        public AsyncViewModelCommand<ApplicationViewModel> EditApplicationCommand { get; private set; }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                SetProperty(ref _Name, value);
            }
        }

        private string _Path;
        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                SetProperty(ref _Path, value);
            }
        }

        private Guid _FolderId;
        public Guid FolderId
        {
            get
            {
                return _FolderId;
            }

            set
            {
                SetProperty(ref _FolderId, value);
            }
        }

        private bool _EnableSilentInstallation;
        public bool EnableSilentInstallation
        {
            get
            {
                return _EnableSilentInstallation;
            }

            set
            {
                SetProperty(ref _EnableSilentInstallation, value);
            }
        }

        private readonly ObservableCollection<InstallerBundleViewModel> _InstallerBundles = new ObservableCollection<InstallerBundleViewModel>();
        public ObservableCollection<InstallerBundleViewModel> InstallerBundles
        {
            get
            {
                return _InstallerBundles;
            }
        }

        private void InstallerBundles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsDirty = true;
        }

        protected override void OnIsDirtyChanged(bool newValue)
        {
            if (newValue)
                return;

            foreach (var installerBundle in InstallerBundles)
                installerBundle.IsDirty = false;
        }

        private InstallerBundleViewModel _SelectedInstallerBundle;
        public InstallerBundleViewModel SelectedInstallerBundle
        {
            get
            {
                return _SelectedInstallerBundle;
            }

            set
            {
                SetProperty(ref _SelectedInstallerBundle, value);
            }
        }
    }
}
