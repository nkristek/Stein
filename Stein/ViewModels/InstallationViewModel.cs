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
        /// <summary>
        /// Current state of the installation
        /// </summary>
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
        /// <summary>
        /// Total count of operations of the current installation
        /// </summary>
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
        /// <summary>
        /// At which installer the current operation is
        /// </summary>
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

        /// <summary>
        /// Returns a string representing the current progress
        /// </summary>
        [PropertySource(nameof(CurrentIndex), nameof(InstallerCount))]
        public string ProgressString
        {
            get
            {
                return String.Format("{0}/{1}", CurrentIndex, InstallerCount);
            }
        }

        private string _Name;
        /// <summary>
        /// Name of the current installer
        /// </summary>
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
