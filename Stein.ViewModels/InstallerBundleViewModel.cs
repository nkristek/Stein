using System;
using System.Collections.ObjectModel;
using System.Linq;
using NKristek.Smaragd.ViewModels;
using Stein.Localizations;

namespace Stein.ViewModels
{
    public sealed class InstallerBundleViewModel
        : ViewModel
    {
        private string _name;

        /// <summary>
        /// The name of the folder of the installer bundle
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private string _path;

        /// <summary>
        /// The full path to the folder of the installer bundle
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value, out _);
        }

        /// <summary>
        /// List of installers in this installer bundle
        /// </summary>
        public ObservableCollection<InstallerViewModel> Installers { get; } = new ObservableCollection<InstallerViewModel>();

        /// <summary>
        /// Gets the culture of the installers
        /// </summary>
        public string Culture => Installers.FirstOrDefault(i => !String.IsNullOrEmpty(i.Culture))?.Culture;

        /// <summary>
        /// Returns the newest creation time of all installers
        /// </summary>
        public DateTime? NewestInstallerCreationTime => Installers.Select(i => i.Created).Max();
        
        /// <summary>
        /// Returns how many installers are installed as a localized string
        /// </summary>
        public string InstalledSummary => $"{Installers.Count(i => i.IsInstalled == true)}/{Installers.Count} {Strings.Installed}";
    }
}
