using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class InstallationResultDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        public override string Title => Name;

        private string _name;

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private string _logFolderPath;

        public string LogFolderPath
        {
            get => _logFolderPath;
            set => SetProperty(ref _logFolderPath, value, out _);
        }

        /// <summary>
        /// The results of each operation.
        /// </summary>
        public ObservableCollection<InstallationResultViewModel> InstallationResults { get; } = new ObservableCollection<InstallationResultViewModel>();
    }
}
