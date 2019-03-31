using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class InstallerBundleViewModel
        : ViewModel
    {
        private string _name;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private DateTime _created;
        
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value, out _);
        }
        
        public ObservableCollection<InstallerViewModel> Installers { get; } = new ObservableCollection<InstallerViewModel>();
    }
}
