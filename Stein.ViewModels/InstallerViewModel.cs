using System;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using Stein.ViewModels.Commands.InstallerViewModelCommands;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public class InstallerViewModel
        : ViewModel
    {
        public InstallerViewModel()
        {
            PreferNothingCommand = new PreferNothingCommand(this);
            PreferInstallCommand = new PreferInstallCommand(this);
            PreferReinstallCommand = new PreferReinstallCommand(this);
            PreferUninstallCommand = new PreferUninstallCommand(this);
        }

        [CommandCanExecuteSource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferNothingCommand { get; }

        [CommandCanExecuteSource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferInstallCommand { get; }

        [CommandCanExecuteSource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferReinstallCommand { get; }

        [CommandCanExecuteSource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferUninstallCommand { get; }
        
        private string _path;
        /// <summary>
        /// FilePath of the installer
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        private InstallerOperationType _preferredOperation;
        /// <summary>
        /// If the installer is enabled by the user
        /// </summary>
        public InstallerOperationType PreferredOperation
        {
            get => _preferredOperation;
            set => SetProperty(ref _preferredOperation, value);
        }
        
        /// <summary>
        /// If the installer is disabled by the system (for example when it isn't installed)
        /// </summary>
        [PropertySource(nameof(IsInstalled))]
        public bool IsDisabled => IsInstalled == null;

        private string _name;
        /// <summary>
        /// Name of the installer (from the Msi-properties)
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Version _version;
        /// <summary>
        /// Version of the installer (from the Msi-properties)
        /// </summary>
        public Version Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private string _culture;
        /// <summary>
        /// Culture in IetfLanguageTag-format of the installer (from the Msi-properties)
        /// </summary>
        public string Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        private string _productCode;
        /// <summary>
        /// ProductCode of the installer (from the Msi-properties)
        /// </summary>
        public string ProductCode
        {
            get => _productCode;
            set => SetProperty(ref _productCode, value);
        }

        private bool? _isInstalled;
        /// <summary>
        /// If the installer is installed
        /// </summary>
        public bool? IsInstalled
        {
            get => _isInstalled;
            set
            {
                if (SetProperty(ref _isInstalled, value))
                    PreferredOperation = InstallerOperationType.DoNothing;
            }
        }

        private DateTime? _created;
        /// <summary>
        /// When the installer file was created
        /// </summary>
        public DateTime? Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }
    }
}
