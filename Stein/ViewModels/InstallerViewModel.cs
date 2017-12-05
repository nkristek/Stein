using Stein.Commands.InstallerViewModelCommands;
using System;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace Stein.ViewModels
{
    public class InstallerViewModel
        : ViewModel
    {
        public InstallerViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            PreferNothingCommand = new PreferNothingCommand(this);
            PreferInstallCommand = new PreferInstallCommand(this);
            PreferReinstallCommand = new PreferReinstallCommand(this);
            PreferUninstallCommand = new PreferUninstallCommand(this);
        }
        
        [PropertySource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferNothingCommand { get; private set; }

        [PropertySource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferInstallCommand { get; private set; }

        [PropertySource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferReinstallCommand { get; private set; }

        [PropertySource(nameof(PreferredOperation), nameof(IsInstalled))]
        public ViewModelCommand<InstallerViewModel> PreferUninstallCommand { get; private set; }
        
        private string _Path;
        /// <summary>
        /// FilePath of the installer
        /// </summary>
        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                SetProperty(ref _Path, value);
            }
        }

        private InstallerOperationType _PreferredOperation;
        /// <summary>
        /// If the installer is enabled by the user
        /// </summary>
        public InstallerOperationType PreferredOperation
        {
            get
            {
                return _PreferredOperation;
            }

            set
            {
                SetProperty(ref _PreferredOperation, value);
            }
        }
        
        /// <summary>
        /// If the installer is disabled by the system (for example when it isn't installed)
        /// </summary>
        [PropertySource(nameof(IsInstalled))]
        public bool IsDisabled
        {
            get
            {
                return IsInstalled == null;
            }
        }

        private string _Name;
        /// <summary>
        /// Name of the installer (from the Msi-properties)
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

        private Version _Version;
        /// <summary>
        /// Version of the installer (from the Msi-properties)
        /// </summary>
        public Version Version
        {
            get
            {
                return _Version;
            }

            set
            {
                SetProperty(ref _Version, value);
            }
        }

        private string _Culture;
        /// <summary>
        /// Culture in IetfLanguageTag-format of the installer (from the Msi-properties)
        /// </summary>
        public string Culture
        {
            get
            {
                return _Culture;
            }

            set
            {
                SetProperty(ref _Culture, value);
            }
        }

        private string _ProductCode;
        /// <summary>
        /// ProductCode of the installer (from the Msi-properties)
        /// </summary>
        public string ProductCode
        {
            get
            {
                return _ProductCode;
            }

            set
            {
                SetProperty(ref _ProductCode, value);
            }
        }

        private bool? _IsInstalled;
        /// <summary>
        /// If the installer is installed
        /// </summary>
        public bool? IsInstalled
        {
            get
            {
                return _IsInstalled;
            }

            set
            {
                if (SetProperty(ref _IsInstalled, value))
                    PreferredOperation = InstallerOperationType.DoNothing;
            }
        }

        private DateTime? _Created;
        /// <summary>
        /// When the installer file was created
        /// </summary>
        public DateTime? Created
        {
            get
            {
                return _Created;
            }

            set
            {
                SetProperty(ref _Created, value);
            }
        }
    }
}
