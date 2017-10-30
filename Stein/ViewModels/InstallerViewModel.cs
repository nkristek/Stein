using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.Services;
using WpfBase.ViewModels;
using System.IO;

namespace Stein.ViewModels
{
    public class InstallerViewModel
        : ViewModel
    {
        public InstallerViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }
        
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

        private bool _IsEnabled;
        /// <summary>
        /// If the installer is enabled by the user
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }

            set
            {
                SetProperty(ref _IsEnabled, value);
            }
        }

        private bool _IsDisabled;
        /// <summary>
        /// If the installer is disabled by the system (for example when it isn't installed)
        /// </summary>
        public bool IsDisabled
        {
            get
            {
                return _IsDisabled;
            }

            set
            {
                SetProperty(ref _IsDisabled, value);
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
                SetProperty(ref _IsInstalled, value);
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
