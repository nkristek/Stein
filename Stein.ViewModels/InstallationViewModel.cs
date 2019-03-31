using System;
using System.ComponentModel;
using System.Threading;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;
using Stein.Presentation;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class InstallationViewModel
        : ViewModel, IDisposable
    {
        private readonly IProgressBarService _progressBarService;

        public InstallationViewModel(IProgressBarService progressBarService)
        {
            _progressBarService = progressBarService ?? throw new ArgumentNullException(nameof(progressBarService));
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Progress) && e.PropertyName != nameof(State))
                return;

            switch (State)
            {
                case InstallationState.Cancelled:
                    _progressBarService.SetState(ProgressBarState.Indeterminate);
                    break;
                case InstallationState.Preparing:
                case InstallationState.Install:
                    _progressBarService.SetState(ProgressBarState.Normal);
                    _progressBarService.SetProgress(Progress);
                    break;
                case InstallationState.Finished:
                    _progressBarService.SetState(ProgressBarState.None);
                    break;
            }
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
                if (!SetProperty(ref _state, value, out _))
                    return;
                if (value == InstallationState.Cancelled)
                    CancellationTokenSource.Cancel();
            }
        }

        private InstallationOperation _currentOperation;

        /// <summary>
        /// Current operation.
        /// </summary>
        public InstallationOperation CurrentOperation
        {
            get => _currentOperation;
            set => SetProperty(ref _currentOperation, value, out _);
        }

        [PropertySource(nameof(CurrentOperation))]
        public bool IsInstalling => CurrentOperation != InstallationOperation.None;
        
        private int _installerCount;

        /// <summary>
        /// Total count of operations of the current installation
        /// </summary>
        public int TotalInstallerFileCount
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

        private int _processedInstallerFileCount;

        public int ProcessedInstallerFileCount
        {
            get => _processedInstallerFileCount;
            set => SetProperty(ref _processedInstallerFileCount, value, out _);
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
        
        /// <summary>
        /// The progress of the installation.
        /// </summary>
        [PropertySource(nameof(ProcessedInstallerFileCount), nameof(TotalInstallerFileCount))]
        public double InstallationProgress => (double)ProcessedInstallerFileCount / TotalInstallerFileCount;

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

        /// <inheritdoc />
        public void Dispose()
        {
            State = InstallationState.Finished;
            _cancellationTokenSource?.Dispose();
        }
    }
}
