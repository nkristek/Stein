using System;

namespace Stein.Common.ProductService
{
    /// <inheritdoc />
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/msi/uninstall-registry-key
    /// </summary>
    public interface IProduct
        : IDisposable
    {
        /// <summary>
        /// ProductCode property
        /// </summary>
        string ProductCode { get; }

        /// <summary>
        /// ProductName property
        /// </summary>
        string DisplayName { get; }
        
        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        string DisplayVersion { get; }

        /// <summary>
        /// Manufacturer property
        /// </summary>
        string Publisher { get; }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        string VersionMinor { get; }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        string VersionMajor { get; }

        /// <summary>
        /// Derived from ProductVersion property
        /// </summary>
        string Version { get; }

        /// <summary>
        /// ARPHELPLINK property
        /// </summary>
        string HelpLink { get; }

        /// <summary>
        /// ARPHELPTELEPHONE property
        /// </summary>
        string HelpTelephone { get; }

        /// <summary>
        /// The last time this product received service.
        /// The value of this property is replaced each time a patch is applied or removed from the product or the /v Command-Line Option is used to repair the product.
        /// If the product has received no repairs or patches this property contains the time this product was installed on this computer.
        /// </summary>
        string InstallDate { get; }

        /// <summary>
        /// ARPINSTALLLOCATION property
        /// </summary>
        string InstallLocation { get; }

        /// <summary>
        /// SourceDir property
        /// </summary>
        string InstallSource { get; }

        /// <summary>
        /// ARPURLINFOABOUT property
        /// </summary>
        string URLInfoAbout { get; }

        /// <summary>
        /// ARPURLUPDATEINFO property
        /// </summary>
        string URLUpdateInfo { get; }

        /// <summary>
        /// ARPAUTHORIZEDCDFPREFIX property
        /// </summary>
        string AuthorizedCDFPrefix { get; }

        /// <summary>
        /// ARPCOMMENTS property
        /// <para/>
        /// Comments provided to the Add or Remove Programs control panel.
        /// </summary>
        string Comments { get; }

        /// <summary>
        /// ARPCONTACT property
        /// <para/>
        /// Contact provided to the Add or Remove Programs control panel.
        /// </summary>
        string Contact { get; }
        
        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        string EstimatedSize { get; }
        
        /// <summary>
        /// ProductLanguage property
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Determined and set by the Windows Installer.
        /// </summary>
        string ModifyPath { get; }

        /// <summary>
        /// ARPREADME property
        /// <para/>
        /// Readme provided to the Add or Remove Programs control panel.
        /// </summary>
        string Readme { get; }

        /// <summary>
        /// Determined and set by Windows Installer.
        /// </summary>
        string UninstallString { get; }

        /// <summary>
        /// MSIARPSETTINGSIDENTIFIER property
        /// </summary>
        string SettingsIdentifier { get; }
    }
}