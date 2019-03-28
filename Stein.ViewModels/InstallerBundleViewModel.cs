using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

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

        private DateTime _created;

        /// <summary>
        /// When the installer file bundle was created.
        /// </summary>
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value, out _);
        }

        /// <summary>
        /// List of installers in this installer bundle
        /// </summary>
        public ObservableCollection<InstallerViewModel> Installers { get; } = new ObservableCollection<InstallerViewModel>();
    }
}
