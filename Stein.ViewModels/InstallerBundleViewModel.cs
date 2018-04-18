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
        private string _name;
        /// <summary>
        /// The name of the folder of the installer bundle
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _Path;
        /// <summary>
        /// The full path to the folder of the installer bundle
        /// </summary>
        public string Path
        {
            get => _Path;
            set => SetProperty(ref _Path, value);
        }

        /// <summary>
        /// List of installers in this installer bundle
        /// </summary>
        public ObservableCollection<InstallerViewModel> Installers { get; } = new ObservableCollection<InstallerViewModel>();

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
                var culture = Installers.FirstOrDefault()?.Culture;
                return Installers.All(i => i.Culture != null && i.Culture == culture) ? culture : null;
            }
        }

        /// <summary>
        /// Returns the newest creation time of all installers
        /// </summary>
        [PropertySource(nameof(Installers))]
        public DateTime? NewestInstallerCreationTime
        {
            get { return Installers.Select(i => i.Created).Max(); }
        }

        /// <summary>
        /// Creates a unique string to identify this InstallerBundleViewModel
        /// </summary>
        /// <returns>A unique string to identify this InstallerBundleViewModel</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Name);

            if (Culture == null)
                return builder.ToString();

            builder.Append(" - ");
            builder.Append(Culture);

            return builder.ToString();
        }
    }
}
