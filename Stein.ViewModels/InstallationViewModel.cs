using System.Diagnostics;
using System.Threading;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class InstallationViewModel
        : ViewModel
    {
        public InstallationViewModel()
        {
            PropertyChanged += (sender, args) =>
            {
                // TODO
                if (args.PropertyName == nameof(Progress))
                    Debug.Print("Progress: " + Progress);
            };
        }

        private string _name;

        /// <summary>
        /// Name of the current installation.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private InstallationState _state;

        /// <summary>
        /// Current state of the installation. If set to <see cref="InstallationState.Cancelled"/> it will cancel the <see cref="CancellationTokenSource"/> and won't change again.
        /// </summary>
        public InstallationState State
        {
            get => _state;
            set
            {
                if (_state == InstallationState.Cancelled)
                    return;
                if (SetProperty(ref _state, value, out _) && value == InstallationState.Cancelled)
                    CancellationTokenSource.Cancel();
            }
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

        private int _currentInstallerIndex;

        /// <summary>
        /// At which installer the current operation is
        /// </summary>
        public int CurrentInstallerIndex
        {
            get => _currentInstallerIndex;
            set => SetProperty(ref _currentInstallerIndex, value, out _);
        }

        private double _downloadProgress;

        /// <summary>
        /// The progress of the file download.
        /// </summary>
        public double DownloadProgress
        {
            get => _downloadProgress;
            set => SetProperty(ref _downloadProgress, value, out _);
        }

        private double _installationProgress;

        /// <summary>
        /// The progress of the installation.
        /// </summary>
        public double InstallationProgress
        {
            get => _installationProgress;
            set => SetProperty(ref _installationProgress, value, out _);
        }

        /// <summary>
        /// Progress of the entire operation.
        /// </summary>
        [PropertySource(nameof(DownloadProgress), nameof(InstallationProgress))]
        public double Progress => (DownloadProgress + InstallationProgress) / 2;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Source for cancellation tokens. Used to cancel the current installation.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource
        {
            get => _cancellationTokenSource;
            set => SetProperty(ref _cancellationTokenSource, value, out _);
        }
    }
}
