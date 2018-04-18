using Microsoft.Win32;
using Stein.Types;

namespace Stein.Services.Types
{
    public class InstalledProgram
        : Disposable
    {
        /// <summary>
        /// Registry key to the program
        /// </summary>
        public RegistryKey RegistryKey { get; set; }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            RegistryKey?.Dispose();
        }

        /// <summary>
        /// ProductName property
        /// </summary>
        public string DisplayName => RegistryKey?.GetValue(nameof(DisplayName)) as string;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string DisplayVersion => RegistryKey?.GetValue(nameof(DisplayVersion)) as string;

        /// <summary>
        /// Manufacturer property
        /// </summary>
        public string Publisher => RegistryKey?.GetValue(nameof(Publisher)) as string;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string VersionMinor => RegistryKey?.GetValue(nameof(VersionMinor)) as string;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string VersionMajor => RegistryKey?.GetValue(nameof(VersionMajor)) as string;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string Version => RegistryKey?.GetValue(nameof(Version)) as string;

        /// <summary>
        /// ARPHELPLINK property
        /// </summary>
        public string HelpLink => RegistryKey?.GetValue(nameof(HelpLink)) as string;

        /// <summary>
        /// ARPHELPTELEPHONE property
        /// </summary>
        public string HelpTelephone => RegistryKey?.GetValue(nameof(HelpTelephone)) as string;

        /// <summary>
        /// The last time this product received service. 
        /// The value of this property is replaced each time a patch is applied or removed from
        /// the product or the /v Command-Line Option is used to repair the product.
        /// If the product has received no repairs or patches this property contains
        /// the time this product was installed on this computer.
        /// </summary>
        public string InstallDate => RegistryKey?.GetValue(nameof(InstallDate)) as string;

        /// <summary>
        /// ARPINSTALLLOCATION property
        /// </summary>
        public string InstallLocation => RegistryKey?.GetValue(nameof(InstallLocation)) as string;

        /// <summary>
        /// SourceDir property
        /// </summary>
        public string InstallSource => RegistryKey?.GetValue(nameof(InstallSource)) as string;

        /// <summary>
        /// ARPURLINFOABOUT property
        /// </summary>
        public string URLInfoAbout => RegistryKey?.GetValue(nameof(URLInfoAbout)) as string;

        /// <summary>
        /// ARPURLUPDATEINFO property
        /// </summary>
        public string URLUpdateInfo => RegistryKey?.GetValue(nameof(URLUpdateInfo)) as string;

        /// <summary>
        /// ARPAUTHORIZEDCDFPREFIX property
        /// </summary>
        public string AuthorizedCDFPrefix => RegistryKey?.GetValue(nameof(AuthorizedCDFPrefix)) as string;

        /// <summary>
        /// Comments provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Comments => RegistryKey?.GetValue(nameof(Comments)) as string;

        /// <summary>
        /// Contact provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Contact => RegistryKey?.GetValue(nameof(Contact)) as string;

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        public string EstimatedSize => RegistryKey?.GetValue(nameof(EstimatedSize)) as string;

        /// <summary>
        /// ProductLanguage property
        /// </summary>
        public string Language => RegistryKey?.GetValue(nameof(Language)) as string;

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        public string ModifyPath => RegistryKey?.GetValue(nameof(ModifyPath)) as string;

        /// <summary>
        /// Readme provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Readme => RegistryKey?.GetValue(nameof(Readme)) as string;

        /// <summary>
        /// Determined and set by Windows Installer.
        /// </summary>
        public string UninstallString => RegistryKey?.GetValue(nameof(UninstallString)) as string;

        /// <summary>
        /// MSIARPSETTINGSIDENTIFIER property
        /// </summary>
        public string SettingsIdentifier => RegistryKey?.GetValue(nameof(SettingsIdentifier)) as string;
    }
}
