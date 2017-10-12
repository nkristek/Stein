using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBase.ViewModels;

namespace Stein.ViewModels
{
    public class InstallationViewModel
        : ViewModel
    {
        public InstallationViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }

        public enum InstallationType
        {
            Install,

            Uninstall
        }

        private InstallationType _Type;

        public InstallationType Type
        {
            get
            {
                return _Type;
            }

            set
            {
                SetProperty(ref _Type, value);
            }
        }
        
        private int _InstallerCount;

        public int InstallerCount
        {
            get
            {
                return _InstallerCount;
            }

            set
            {
                SetProperty(ref _InstallerCount, value);
            }
        }

        private int _CurrentIndex;

        public int CurrentIndex
        {
            get
            {
                return _CurrentIndex;
            }

            set
            {
                SetProperty(ref _CurrentIndex, value);
            }
        }

        [PropertySource(nameof(CurrentIndex), nameof(InstallerCount))]
        public string ProgressString
        {
            get
            {
                if (CurrentIndex == 0 || InstallerCount == 0) return null;
                return String.Join(String.Empty, CurrentIndex.ToString(), "/", InstallerCount.ToString());
            }
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
    }
}
