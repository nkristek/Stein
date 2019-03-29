using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;
using Stein.Localizations;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class InstallationResultViewModel
        : ViewModel
    {
        private string _installerName;

        /// <summary>
        /// The name of the installer file.
        /// </summary>
        public string InstallerName
        {
            get => _installerName;
            set => SetProperty(ref _installerName, value, out _);
        }
        
        /// <summary>
        /// The file paths to the log files created during installation. Maybe empty if logging was disabled or the download of the file failed.
        /// </summary>
        public ObservableCollection<string> InstallationLogFilePaths { get; } = new ObservableCollection<string>();

        private InstallationResultState _state;

        /// <summary>
        /// The state of the installation.
        /// </summary>
        public InstallationResultState State
        {
            get => _state;
            set
            {
                if (!SetProperty(ref _state, value, out _))
                    return;

                SetLocalizedReason();
            }
        }

        private bool _isExceptionVisible;
        
        public bool IsExceptionVisible
        {
            get => _isExceptionVisible;
            set => SetProperty(ref _isExceptionVisible, value, out _);
        }

        private ExceptionViewModel _exception;

        /// <summary>
        /// Contains information about an <see cref="Exception"/> that was thrown while downloading/installing the corresponding installer file. Maybe <c>null</c> if no exception occured.
        /// </summary>
        public ExceptionViewModel Exception
        {
            get => _exception;
            set
            {
                if (!SetProperty(ref _exception, value, out _))
                    return;

                SetLocalizedReason();
            }
        }

        private void SetLocalizedReason()
        {
            if (Exception == null)
                return;

            var isReadOnly = Exception.IsReadOnly;
            Exception.IsReadOnly = false;
            switch (State)
            {
                case InstallationResultState.Success:
                    Exception.LocalizedReason = Strings.InstallationSuccess;
                    break;
                case InstallationResultState.Skipped:
                    Exception.LocalizedReason = Strings.InstallationSkipped;
                    break;
                case InstallationResultState.Cancelled:
                    Exception.LocalizedReason = Strings.InstallationCancelled;
                    break;
                case InstallationResultState.DownloadFailed:
                    Exception.LocalizedReason = Strings.InstallationDownloadFailed;
                    break;
                case InstallationResultState.InstallationFailed:
                    Exception.LocalizedReason = Strings.InstallationFailed;
                    break;
            }
            Exception.IsReadOnly = isReadOnly;
        }
    }
}
