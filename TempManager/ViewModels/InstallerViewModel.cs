using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempManager.Services;
using WpfBase.ViewModels;

namespace TempManager.ViewModels
{
    public class InstallerViewModel
        : ViewModel
    {
        public InstallerViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }

        private string _Name;

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

        private string _Path;

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

        private Version _Version;

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

        private bool _IsInstalled;

        public bool IsInstalled
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

        private Dictionary<string, string> _MsiProperties;

        public Dictionary<string, string> MsiProperties
        {
            get
            {
                return _MsiProperties;
            }

            set
            {
                SetProperty(ref _MsiProperties, value);
            }
        }

        private InstalledProgram _InstalledProgram;

        public InstalledProgram InstalledProgram
        {
            get
            {
                return _InstalledProgram;
            }

            set
            {
                SetProperty(ref _InstalledProgram, value);
            }
        }
    }
}
