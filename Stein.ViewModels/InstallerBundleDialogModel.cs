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
        
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value, out _))
                    Title = value;
            }
        }
        
        public string Culture
        {
            get
            {
                return !Installers.Any() ? String.Empty : String.Join(", ", Installers.Where(i => !String.IsNullOrWhiteSpace(i.Culture)).Select(i => i.Culture).Distinct());
            }
        }

        public ObservableCollection<InstallerViewModel> Installers => (Parent as InstallerBundleViewModel)?.Installers;
    }
}
