using System;
using Microsoft.Win32;
using Stein.Helpers;

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

        private string _displayName;

        /// <summary>
        /// ProductName property
        /// </summary>
        public string DisplayName => _displayName ?? (_displayName = RegistryKey?.GetValue(nameof(DisplayName)) as string ?? String.Empty);

        private string _displayVersion;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string DisplayVersion => _displayVersion ?? (_displayVersion = RegistryKey?.GetValue(nameof(DisplayVersion)) as string ?? String.Empty);

        private string _publisher;

        /// <summary>
        /// Manufacturer property
        /// </summary>
        public string Publisher => _publisher ?? (_publisher = RegistryKey?.GetValue(nameof(Publisher)) as string ?? String.Empty);

        private string _versionMinor;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string VersionMinor => _versionMinor ?? (_versionMinor = RegistryKey?.GetValue(nameof(VersionMinor)) as string ?? String.Empty);

        private string _versionMajor;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string VersionMajor => _versionMajor ?? (_versionMajor = RegistryKey?.GetValue(nameof(VersionMajor)) as string ?? String.Empty);

        private string _version;

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        public string Version => _version ?? (_version = RegistryKey?.GetValue(nameof(Version)) as string ?? String.Empty);

        private string _helpLink;

        /// <summary>
        /// ARPHELPLINK property
        /// </summary>
        public string HelpLink => _helpLink ?? (_helpLink = RegistryKey?.GetValue(nameof(HelpLink)) as string ?? String.Empty);

        private string _helpTelephone;

        /// <summary>
        /// ARPHELPTELEPHONE property
        /// </summary>
        public string HelpTelephone => _helpTelephone ?? (_helpTelephone = RegistryKey?.GetValue(nameof(HelpTelephone)) as string ?? String.Empty);

        private string _installDate;

        /// <summary>
        /// The last time this product received service. 
        /// The value of this property is replaced each time a patch is applied or removed from
        /// the product or the /v Command-Line Option is used to repair the product.
        /// If the product has received no repairs or patches this property contains
        /// the time this product was installed on this computer.
        /// </summary>
        public string InstallDate => _installDate ?? (_installDate = RegistryKey?.GetValue(nameof(InstallDate)) as string ?? String.Empty);

        private string _installLocation;

        /// <summary>
        /// ARPINSTALLLOCATION property
        /// </summary>
        public string InstallLocation => _installLocation ?? (_installLocation = RegistryKey?.GetValue(nameof(InstallLocation)) as string ?? String.Empty);

        private string _installSource;

        /// <summary>
        /// SourceDir property
        /// </summary>
        public string InstallSource => _installSource ?? (_installSource = RegistryKey?.GetValue(nameof(InstallSource)) as string ?? String.Empty);

        private string _urlInfoAbout;

        /// <summary>
        /// ARPURLINFOABOUT property
        /// </summary>
        public string URLInfoAbout => _urlInfoAbout ?? (_urlInfoAbout = RegistryKey?.GetValue(nameof(URLInfoAbout)) as string ?? String.Empty);

        private string _urlUpdateInfo;

        /// <summary>
        /// ARPURLUPDATEINFO property
        /// </summary>
        public string URLUpdateInfo => _urlUpdateInfo ?? (_urlUpdateInfo = RegistryKey?.GetValue(nameof(URLUpdateInfo)) as string ?? String.Empty);

        private string _authorizedCDFPrefix;

        /// <summary>
        /// ARPAUTHORIZEDCDFPREFIX property
        /// </summary>
        public string AuthorizedCDFPrefix => _authorizedCDFPrefix ?? (_authorizedCDFPrefix = RegistryKey?.GetValue(nameof(AuthorizedCDFPrefix)) as string ?? String.Empty);

        private string _comments;

        /// <summary>
        /// Comments provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Comments => _comments ?? (_comments = RegistryKey?.GetValue(nameof(Comments)) as string ?? String.Empty);

        private string _contact;

        /// <summary>
        /// Contact provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Contact => _contact ?? (_contact = RegistryKey?.GetValue(nameof(Contact)) as string ?? String.Empty);

        private string _estimatedSize;

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        public string EstimatedSize => _estimatedSize ?? (_estimatedSize = RegistryKey?.GetValue(nameof(EstimatedSize)) as string ?? String.Empty);

        private string _language;

        /// <summary>
        /// ProductLanguage property
        /// </summary>
        public string Language => _language ?? (_language = RegistryKey?.GetValue(nameof(Language)) as string ?? String.Empty);

        private string _modifyPath;

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        public string ModifyPath => _modifyPath ?? (_modifyPath = RegistryKey?.GetValue(nameof(ModifyPath)) as string ?? String.Empty);

        private string _readme;

        /// <summary>
        /// Readme provided to the Add or Remove Programs control panel.
        /// </summary>
        public string Readme => _readme ?? (_readme = RegistryKey?.GetValue(nameof(Readme)) as string ?? String.Empty);

        private string _uninstallString;

        /// <summary>
        /// Determined and set by Windows Installer.
        /// </summary>
        public string UninstallString => _uninstallString ?? (_uninstallString = RegistryKey?.GetValue(nameof(UninstallString)) as string ?? String.Empty);

        private string _settingsIdentifier;

        /// <summary>
        /// MSIARPSETTINGSIDENTIFIER property
        /// </summary>
        public string SettingsIdentifier => _settingsIdentifier ?? (_settingsIdentifier = RegistryKey?.GetValue(nameof(SettingsIdentifier)) as string ?? String.Empty);
    }
}
