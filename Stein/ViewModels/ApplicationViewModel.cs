using System;
using System.Collections.ObjectModel;
using Stein.Commands.ApplicationViewModelCommands;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace Stein.ViewModels
{
    public class ApplicationViewModel
        : ViewModel
    {
        public ApplicationViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            ModifyApplicationCommand = new ModifyApplicationCommand(this);
            SelectFolderCommand = new SelectFolderCommand(this);
            OpenLogFolderCommand = new OpenLogFolderCommand(this);

            InstallerBundles.CollectionChanged += InstallerBundles_CollectionChanged;
        }

        [CommandCanExecuteSource(nameof(Parent), nameof(SelectedInstallerBundle))]
        public AsyncViewModelCommand<ApplicationViewModel> ModifyApplicationCommand { get; private set; }
        
        public ViewModelCommand<ApplicationViewModel> SelectFolderCommand { get; private set; }

        public ViewModelCommand<ApplicationViewModel> OpenLogFolderCommand { get; private set; }
        
        private string _Name;
        /// <summary>
        /// Name of the application folder
        /// </summary>
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
        /// <summary>
        /// Path to the application folder
        /// </summary>
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
        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
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
        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
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

        private bool _EnableInstallationLogging;
        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get
            {
                return _EnableInstallationLogging;
            }

            set
            {
                SetProperty(ref _EnableInstallationLogging, value);
            }
        }

        private readonly ObservableCollection<InstallerBundleViewModel> _InstallerBundles = new ObservableCollection<InstallerBundleViewModel>();
        /// <summary>
        /// List of all installer bundles of this application
        /// </summary>
        public ObservableCollection<InstallerBundleViewModel> InstallerBundles
        {
            get
            {
                return _InstallerBundles;
            }
        }

        /// <summary>
        /// Attach property changed handler to elements in the collection to raise a PropertyChanged event if an element changed
        /// </summary>
        /// <param name="sender">The collection</param>
        /// <param name="e">EventArgs</param>
        private void InstallerBundles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsDirty = true;

            if (e.NewItems != null)
                foreach (var newItem in e.NewItems)
                    if (newItem is InstallerBundleViewModel installerBundle)
                        installerBundle.PropertyChanged += InstallerBundle_PropertyChanged;

            if (e.OldItems != null)
                foreach (var oldItem in e.OldItems)
                    if (oldItem is InstallerBundleViewModel installerBundle)
                        installerBundle.PropertyChanged -= InstallerBundle_PropertyChanged;
        }

        /// <summary>
        /// Raise a PropertyChanged event for the collection if a property changed on the item of the collection
        /// </summary>
        /// <param name="sender">Item of the collection</param>
        /// <param name="e">EventArgs</param>
        private void InstallerBundle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                RaisePropertyChanged(nameof(InstallerBundles));
        }

        private InstallerBundleViewModel _SelectedInstallerBundle;
        /// <summary>
        /// The currently selected InstallerBundleViewModel
        /// </summary>
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
