using Microsoft.Win32;
using System;
using nkristek.MVVMBase;

namespace Stein.Services
{
    public class InstalledProgram
        : Disposable
    {
        /// <summary>
        /// Registry key to the program
        /// </summary>
        public RegistryKey RegistryKey { get; set; }

        /// <summary>
        /// Disposable
        /// </summary>
        protected override void DisposeManagedResources()
        {
            RegistryKey?.Dispose();
        }

        /// <summary>
        /// ProductName property
        /// </summary>
        public string DisplayName
        {
            get
            {
                return RegistryKey?.GetValue("DisplayName") as string;
            }
        }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string DisplayVersion
        {
            get
            {
                return RegistryKey?.GetValue("DisplayVersion") as string;
            }
        }

        /// <summary>
        /// Manufacturer property
        /// </summary>
        public string Publisher
        {
            get
            {
                return RegistryKey?.GetValue("Publisher") as string;
            }
        }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string VersionMinor
        {
            get
            {
                return RegistryKey?.GetValue("VersionMinor") as string;
            }
        }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string VersionMajor
        {
            get
            {
                return RegistryKey?.GetValue("VersionMajor") as string;
            }
        }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string Version
        {
            get
            {
                return RegistryKey?.GetValue("Version") as string;
            }
        }

        /// <summary>
        /// ARPHELPLINK property
        /// </summary>
        public string HelpLink
        {
            get
            {
                return RegistryKey?.GetValue("HelpLink") as string;
            }
        }

        /// <summary>
        /// ARPHELPTELEPHONE property
        /// </summary>
        public string HelpTelephone
        {
            get
            {
                return RegistryKey?.GetValue("HelpTelephone") as string;
            }
        }

        /// <summary>
        /// The last time this product received service. 
        /// The value of this property is replaced each time a patch is applied or removed from
        /// the product or the /v Command-Line Option is used to repair the product.
        /// If the product has received no repairs or patches this property contains
        /// the time this product was installed on this computer.
        /// </summary>
        public string InstallDate
        {
            get
            {
                return RegistryKey?.GetValue("InstallDate") as string;
            }
        }

        /// <summary>
        /// ARPINSTALLLOCATION property
        /// </summary>
        public string InstallLocation
        {
            get
            {
                return RegistryKey?.GetValue("InstallLocation") as string;
            }
        }

        /// <summary>
        /// SourceDir property
        /// </summary>
        public string InstallSource
        {
            get
            {
                return RegistryKey?.GetValue("InstallSource") as string;
            }
        }

        /// <summary>
        /// ARPURLINFOABOUT property
        /// </summary>
        public string URLInfoAbout
        {
            get
            {
                return RegistryKey?.GetValue("URLInfoAbout") as string;
            }
        }

        /// <summary>
        /// ARPURLUPDATEINFO property
        /// </summary>
        public string URLUpdateInfo
        {
            get
            {
                return RegistryKey?.GetValue("URLUpdateInfo") as string;
            }
        }

        /// <summary>
        /// ARPAUTHORIZEDCDFPREFIX property
        /// </summary>
        public string AuthorizedCDFPrefix
        {
            get
            {
                return RegistryKey?.GetValue("AuthorizedCDFPrefix") as string;
            }
        }

        /// <summary>
        /// Comments provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Comments
        {
            get
            {
                return RegistryKey?.GetValue("Comments") as string;
            }
        }

        /// <summary>
        /// Contact provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Contact
        {
            get
            {
                return RegistryKey?.GetValue("Contact") as string;
            }
        }

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        public string EstimatedSize
        {
            get
            {
                return RegistryKey?.GetValue("EstimatedSize") as string;
            }
        }

        /// <summary>
        /// ProductLanguage property
        /// </summary>
        public string Language
        {
            get
            {
                return RegistryKey?.GetValue("Language") as string;
            }
        }

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        public string ModifyPath
        {
            get
            {
                return RegistryKey?.GetValue("ModifyPath") as string;
            }
        }

        /// <summary>
        /// Readme provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Readme
        {
            get
            {
                return RegistryKey?.GetValue("Readme") as string;
            }
        }

        /// <summary>
        /// Determined and set by Windows Installer.
        /// </summary>
        public string UninstallString
        {
            get
            {
                return RegistryKey?.GetValue("UninstallString") as string;
            }
        }

        /// <summary>
        /// MSIARPSETTINGSIDENTIFIER property
        /// </summary>
        public string SettingsIdentifier
        {
            get
            {
                return RegistryKey?.GetValue("SettingsIdentifier") as string;
            }
        }
    }
}
