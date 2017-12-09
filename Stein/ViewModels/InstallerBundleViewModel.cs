using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using nkristek.MVVMBase.ViewModels;

namespace Stein.ViewModels
{
    public class InstallerBundleViewModel
        : ViewModel
    {
        public InstallerBundleViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            Installers.CollectionChanged += Installers_CollectionChanged;
        }

        private string _Name;
        /// <summary>
        /// The name of the folder of the installer bundle
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
        /// The full path to the folder of the installer bundle
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

        /// <summary>
        /// Gets the culture of the installers, if the Culture property is the same on all Installers, otherwise null
        /// </summary>
        [PropertySource(nameof(Installers))]
        public string Culture
        {
            get
            {
                if (!Installers.Any())
                    return null;

                var culture = Installers.FirstOrDefault().Culture;
                return Installers.All(i => i.Culture != null && i.Culture == culture) ? culture : null;
            }
        }

        /// <summary>
        /// Gets the version of the installers, if the Version property is the same on all Installers, otherwise null
        /// </summary>
        [PropertySource(nameof(Installers))]
        public Version Version
        {
            get
            {
                if (!Installers.Any())
                    return null;

                var version = Installers.FirstOrDefault().Version;
                return Installers.All(i => i.Version != null && i.Version == version) ? version : null;
            }
        }

        private readonly ObservableCollection<InstallerViewModel> _Installers = new ObservableCollection<InstallerViewModel>();
        /// <summary>
        /// List of installers in this installer bundle
        /// </summary>
        public ObservableCollection<InstallerViewModel> Installers
        {
            get
            {
                return _Installers;
            }
        }

        /// <summary>
        /// Attach property changed handler to elements in the collection to raise a PropertyChanged event if an element changed
        /// </summary>
        /// <param name="sender">The collection</param>
        /// <param name="e">EventArgs</param>
        private void Installers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsDirty = true;

            if (e.NewItems != null)
                foreach (var newItem in e.NewItems)
                    if (newItem is InstallerViewModel installer)
                        installer.PropertyChanged += Installer_PropertyChanged;

            if (e.OldItems != null)
                foreach (var oldItem in e.OldItems)
                    if (oldItem is InstallerViewModel installer)
                        installer.PropertyChanged -= Installer_PropertyChanged;
        }

        /// <summary>
        /// Raise a PropertyChanged event for the collection if a property changed on the item of the collection
        /// </summary>
        /// <param name="sender">Item of the collection</param>
        /// <param name="e">EventArgs</param>
        private void Installer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                RaisePropertyChanged(nameof(Installers));
        }

        /// <summary>
        /// Returns if any installer is enabled
        /// </summary>
        [PropertySource(nameof(Installers))]
        public bool AnyOperationWillBeExecuted
        {
            get
            {
                return Installers.Any(i => i.PreferredOperation != InstallerOperationType.DoNothing);
            }
        }

        /// <summary>
        /// Returns the newest creation time of all installers
        /// </summary>
        [PropertySource(nameof(Installers))]
        public DateTime? NewestInstallerCreationTime
        {
            get
            {
                return Installers.Select(i => i.Created).Max();
            }
        }

        /// <summary>
        /// Creates a unique string to identify this InstallerBundleViewModel
        /// </summary>
        /// <returns>A unique string to identify this InstallerBundleViewModel</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Name);

            if (Culture != null)
            {
                builder.Append(" - ");
                builder.Append(Culture);
            }

            return builder.ToString();
        }
    }
}
