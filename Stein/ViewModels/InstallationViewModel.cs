using System;
using WpfBase.ViewModels;

namespace Stein.ViewModels
{
    public class InstallationViewModel
        : ViewModel
    {
        public InstallationViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }

        public enum InstallationState
        {
            Preparing,

            Install,

            Reinstall,

            Uninstall,

            Cancelled
        }

        private InstallationState _State;
        public InstallationState State
        {
            get
            {
                return _State;
            }

            set
            {
                SetProperty(ref _State, value);
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
