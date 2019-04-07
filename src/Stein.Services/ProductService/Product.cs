using System;
using System.IO;
using Microsoft.Win32;
using Stein.Utility;

namespace Stein.Services.ProductService
{
    /// <inheritdoc cref="IProduct" />
    public class Product
        : Disposable, IProduct
    {
        /// <summary>
        /// Registry key of the product.
        /// </summary>
        public RegistryKey RegistryKey { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product" /> class with a <see cref="RegistryKey"/> of the product.
        /// </summary>
        /// <param name="registryKey"><see cref="RegistryKey"/> of the product.</param>
        public Product(RegistryKey registryKey)
        {
            RegistryKey = registryKey ?? throw new ArgumentNullException(nameof(registryKey));
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            RegistryKey.Dispose();
        }

        private string _productCode;

        /// <inheritdoc />
        public string ProductCode
        {
            get
            {
                if (_productCode != null)
                    return _productCode;

                var productCodePath = RegistryKey.Name;
                _productCode = Path.GetFileName(productCodePath);

                return _productCode;
            }
        }

        private string _displayName;
        
        /// <inheritdoc />
        public string DisplayName => _displayName ?? (_displayName = RegistryKey.GetValue(nameof(DisplayName)) as string ?? String.Empty);

        private string _displayVersion;

        /// <inheritdoc />
        public string DisplayVersion => _displayVersion ?? (_displayVersion = RegistryKey.GetValue(nameof(DisplayVersion)) as string ?? String.Empty);

        private string _publisher;

        /// <inheritdoc />
        public string Publisher => _publisher ?? (_publisher = RegistryKey.GetValue(nameof(Publisher)) as string ?? String.Empty);

        private string _versionMinor;

        /// <inheritdoc />
        public string VersionMinor => _versionMinor ?? (_versionMinor = RegistryKey.GetValue(nameof(VersionMinor)) as string ?? String.Empty);

        private string _versionMajor;

        /// <inheritdoc />
        public string VersionMajor => _versionMajor ?? (_versionMajor = RegistryKey.GetValue(nameof(VersionMajor)) as string ?? String.Empty);

        private string _version;

        /// <inheritdoc />
        public string Version => _version ?? (_version = RegistryKey.GetValue(nameof(Version)) as string ?? String.Empty);

        private string _helpLink;

        /// <inheritdoc />
        public string HelpLink => _helpLink ?? (_helpLink = RegistryKey.GetValue(nameof(HelpLink)) as string ?? String.Empty);

        private string _helpTelephone;

        /// <inheritdoc />
        public string HelpTelephone => _helpTelephone ?? (_helpTelephone = RegistryKey.GetValue(nameof(HelpTelephone)) as string ?? String.Empty);

        private string _installDate;

        /// <inheritdoc />
        public string InstallDate => _installDate ?? (_installDate = RegistryKey.GetValue(nameof(InstallDate)) as string ?? String.Empty);

        private string _installLocation;

        /// <inheritdoc />
        public string InstallLocation => _installLocation ?? (_installLocation = RegistryKey.GetValue(nameof(InstallLocation)) as string ?? String.Empty);

        private string _installSource;

        /// <inheritdoc />
        public string InstallSource => _installSource ?? (_installSource = RegistryKey.GetValue(nameof(InstallSource)) as string ?? String.Empty);

        private string _urlInfoAbout;

        /// <inheritdoc />
        public string URLInfoAbout => _urlInfoAbout ?? (_urlInfoAbout = RegistryKey.GetValue(nameof(URLInfoAbout)) as string ?? String.Empty);

        private string _urlUpdateInfo;

        /// <inheritdoc />
        public string URLUpdateInfo => _urlUpdateInfo ?? (_urlUpdateInfo = RegistryKey.GetValue(nameof(URLUpdateInfo)) as string ?? String.Empty);

        private string _authorizedCDFPrefix;

        /// <inheritdoc />
        public string AuthorizedCDFPrefix => _authorizedCDFPrefix ?? (_authorizedCDFPrefix = RegistryKey.GetValue(nameof(AuthorizedCDFPrefix)) as string ?? String.Empty);

        private string _comments;

        /// <inheritdoc />
        public string Comments => _comments ?? (_comments = RegistryKey.GetValue(nameof(Comments)) as string ?? String.Empty);

        private string _contact;

        /// <inheritdoc />
        public string Contact => _contact ?? (_contact = RegistryKey.GetValue(nameof(Contact)) as string ?? String.Empty);

        private string _estimatedSize;

        /// <inheritdoc />
        public string EstimatedSize => _estimatedSize ?? (_estimatedSize = RegistryKey.GetValue(nameof(EstimatedSize)) as string ?? String.Empty);

        private string _language;

        /// <inheritdoc />
        public string Language => _language ?? (_language = RegistryKey.GetValue(nameof(Language)) as string ?? String.Empty);

        private string _modifyPath;

        /// <inheritdoc />
        public string ModifyPath => _modifyPath ?? (_modifyPath = RegistryKey.GetValue(nameof(ModifyPath)) as string ?? String.Empty);

        private string _readme;

        /// <inheritdoc />
        public string Readme => _readme ?? (_readme = RegistryKey.GetValue(nameof(Readme)) as string ?? String.Empty);

        private string _uninstallString;

        /// <inheritdoc />
        public string UninstallString => _uninstallString ?? (_uninstallString = RegistryKey.GetValue(nameof(UninstallString)) as string ?? String.Empty);

        private string _settingsIdentifier;

        /// <inheritdoc />
        public string SettingsIdentifier => _settingsIdentifier ?? (_settingsIdentifier = RegistryKey.GetValue(nameof(SettingsIdentifier)) as string ?? String.Empty);
    }
}
