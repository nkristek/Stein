using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
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
            set => SetProperty(ref _name, value);
        }

        private string _logFolderPath;

        public string LogFolderPath
        {
            get => _logFolderPath;
            set => SetProperty(ref _logFolderPath, value);
        }
        
        public ObservableCollection<InstallationResultViewModel> InstallationResults { get; } = new ObservableCollection<InstallationResultViewModel>();

        private IViewModelCommand<InstallationResultDialogModel> _openLogFolderCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<InstallationResultDialogModel> OpenLogFolderCommand
        {
            get => _openLogFolderCommand;
            set
            {
                if (SetProperty(ref _openLogFolderCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }
    }
}
