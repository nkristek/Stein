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
        public string Name
        {
            get
            {
                if (_Name == null)
                    _Name = InstallService.GetPropertyFromMsi(Path, InstallService.MsiPropertyName.ProductName);
                return _Name;
            }
        }

        private Version _Version;
        public Version Version
        {
            get
            {
                if (_Version == null)
                    _Version = InstallService.GetVersionFromMsi(Path);
                return _Version;
            }
        }

        private string _Culture;
        public string Culture
        {
            get
            {
                if (_Culture == null)
                    _Culture = InstallService.GetCultureTagFromMsi(Path);
                return _Culture;
            }
        }

        private bool? _IsInstalled;
        public bool IsInstalled
        {
            get
            {
                if (!_IsInstalled.HasValue)
                    _IsInstalled = InstallService.IsMsiInstalled(Path);
                return _IsInstalled.Value;
            }
        }

        private DateTime _Created;
        public DateTime Created
        {
            get
            {
                if (_Created == DateTime.MinValue)
                    _Created = new FileInfo(Path).CreationTime;
                return _Created;
            }
        }
    }
}
