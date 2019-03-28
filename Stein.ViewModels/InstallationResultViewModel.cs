using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;
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
            set => SetProperty(ref _state, value, out _);
        }

        private ExceptionViewModel _exception;

        /// <summary>
        /// Contains information about an <see cref="Exception"/> that was thrown while downloading/installing the corresponding installer file. Maybe <c>null</c> if no exception occured.
        /// </summary>
        public ExceptionViewModel Exception
        {
            get => _exception;
            set => SetProperty(ref _exception, value, out _);
        }
    }
}
