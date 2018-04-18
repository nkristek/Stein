using nkristek.MVVMBase.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stein.ViewModels
{
    public class InstallerBundleDialogModel
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
                if (SetProperty(ref _name, value))
                    Title = _name;
            }
        }

        private string _path;
        /// <summary>
        /// The full path to the folder of the installer bundle
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
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
    }
}
