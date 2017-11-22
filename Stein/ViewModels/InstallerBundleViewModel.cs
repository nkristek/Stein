using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WpfBase.ViewModels;

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
        public ObservableCollection<InstallerViewModel> Installers
        {
            get
            {
                return _Installers;
            }
        }

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

        private void Installer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                RaisePropertyChanged(nameof(Installers));
        }

        [PropertySource(nameof(Installers))]
        public bool AnyInstallerIsEnabled
        {
            get
            {
                return Installers.Any(i => i.IsEnabled);
            }
        }

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
