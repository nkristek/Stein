using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class InstallationResultDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        [PropertySource(nameof(Name))]
        public override string Title => Name;

        private string _name;
        
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
        
        public ObservableCollection<InstallationResultViewModel> InstallationResults { get; } = new ObservableCollection<InstallationResultViewModel>();
    }
}
