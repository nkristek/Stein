using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class InstallationViewModel
        : ViewModel
    {
        private string _name;
        /// <summary>
        /// Name of the current installer
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private InstallationState _state;
        /// <summary>
        /// Current state of the installation
        /// </summary>
        public InstallationState State
        {
            get => _state;
            set => SetProperty(ref _state, value, out _);
        }
        
        private int _installerCount;
        /// <summary>
        /// Total count of operations of the current installation
        /// </summary>
        public int InstallerCount
        {
            get => _installerCount;
            set => SetProperty(ref _installerCount, value, out _);
        }

        private int _currentIndex;
        /// <summary>
        /// At which installer the current operation is
        /// </summary>
        public int CurrentIndex
        {
            get => _currentIndex;
            set => SetProperty(ref _currentIndex, value, out _);
        }

        /// <summary>
        /// Returns a string representing the current progress
        /// </summary>
        [PropertySource(nameof(CurrentIndex), nameof(InstallerCount))]
        public string ProgressString => $"{CurrentIndex}/{InstallerCount}";
    }
}
