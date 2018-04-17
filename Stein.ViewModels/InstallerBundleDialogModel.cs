using nkristek.MVVMBase.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stein.ViewModels
{
    public class InstallerBundleDialogModel
        : DialogModel
    {
        private string _Name;
        /// <summary>
        /// The name of the folder of the installer bundle
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (SetProperty(ref _Name, value))
                    Title = _Name;
            }
        }

        private string _Path;
        /// <summary>
        /// The full path to the folder of the installer bundle
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { SetProperty(ref _Path, value); }
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
                var culture = Installers.FirstOrDefault().Culture;
                return Installers.All(i => i.Culture != null && i.Culture == culture) ? culture : null;
            }
        }
    }
}
