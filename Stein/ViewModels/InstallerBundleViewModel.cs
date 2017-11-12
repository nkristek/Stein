using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBase.Commands;
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
        }

        protected override void OnIsDirtyChanged(bool newValue)
        {
            if (newValue)
                return;

            foreach (var installer in Installers)
                installer.IsDirty = false;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(Name))
                builder.Append(Name);
            else if (Version != null)
                builder.Append(Version.ToString());
            else
                builder.Append("Error");

            var dateRange = GetDateRangeOfInstallers();
            if (!String.IsNullOrEmpty(dateRange))
            {
                builder.Append(" - ");
                builder.Append(dateRange);
            }

            if (Culture != null)
            {
                builder.Append(" - ");
                builder.Append(Culture);
            }

            return builder.ToString();
        }

        private string GetDateRangeOfInstallers()
        {
            if (Installers.Any())
                return String.Empty;

            var minimum = Installers.Min(i => i.Created);
            var maximum = Installers.Max(i => i.Created);
            if (!minimum.HasValue && !maximum.HasValue)
                return String.Empty;

            var dateStringBuilder = new StringBuilder();

            if (minimum.HasValue)
                dateStringBuilder.Append(minimum.Value.ToShortDateString());

            if (maximum.HasValue)
            {
                if (dateStringBuilder.Length > 0)
                    dateStringBuilder.Append("-");
                dateStringBuilder.Append(maximum.Value.ToShortDateString());
            }

            return dateStringBuilder.ToString();
        }
    }
}
