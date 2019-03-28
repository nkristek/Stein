using System;
using System.Collections.ObjectModel;
using System.Linq;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class InstallerBundleDialogModel
        : DialogModel
    {
        private string _name;

        /// <summary>
        /// The name of the folder of the installer bundle
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value, out _))
                    Title = value;
            }
        }
        
        /// <summary>
        /// List of installers in this installer bundle
        /// </summary>
        public ObservableCollection<InstallerViewModel> Installers => (Parent as InstallerBundleViewModel)?.Installers;

        /// <summary>
        /// Gets the culture of the installers separated by a comma.
        /// </summary>
        public string Culture
        {
            get
            {
                return !Installers.Any() ? String.Empty : String.Join(", ", Installers.Where(i => !String.IsNullOrWhiteSpace(i.Culture)).Select(i => i.Culture).Distinct());
            }
        }
    }
}
