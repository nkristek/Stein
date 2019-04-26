using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Stein.Common.InstallerFiles;
using Stein.Common.MsiService;

namespace Stein.Services.InstallerFiles.Base
{
    public abstract class InstallerFile
        : IInstallerFile
    {
        /// <inheritdoc />
        public string FileName { get; set; }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public DateTime Created { get; set; }

        /// <inheritdoc />
        public CultureInfo Culture { get; private set; }

        /// <inheritdoc />
        public Version Version { get; private set; }

        /// <inheritdoc />
        public string ProductCode { get; private set; }

        /// <inheritdoc />
        public abstract Task SaveFileAsync(string filePath, IMsiService msiService, IProgress<double> progress = null, CancellationToken cancellationToken = default);
        
        protected async Task ReadMsiMetadata(string filePath, IMsiService msiService)
        {
            await Task.Run(() =>
            {
                using (var metadata = msiService.GetMsiMetadata(filePath))
                {
                    Name = metadata.GetProperty(MsiPropertyName.ProductName);
                    if (String.IsNullOrEmpty(Name))
                        throw new Exception($"MSI installer \"{FileName}\" has no ProductName property. This property is REQUIRED according to the official documentation: https://docs.microsoft.com/en-us/windows/desktop/msi/productname");
                    
                    var languageString = metadata.GetProperty(MsiPropertyName.ProductLanguage);
                    if (String.IsNullOrEmpty(languageString))
                        throw new Exception($"MSI installer \"{FileName}\" has no ProductLanguage property. This property is REQUIRED according to the official documentation: https://docs.microsoft.com/en-us/windows/desktop/msi/productlanguage");
                    if (!int.TryParse(languageString, out var languageId))
                        throw new Exception($"Parsing the ProductLanguage of MSI installer \"{FileName}\" failed. (got: {languageString})");
                    Culture = new CultureInfo(languageId);

                    var versionString = metadata.GetProperty(MsiPropertyName.ProductVersion);
                    if (String.IsNullOrEmpty(versionString))
                        throw new Exception($"MSI installer \"{FileName}\" has no ProductVersion property. This property is REQUIRED according to the official documentation: https://docs.microsoft.com/en-us/windows/desktop/msi/productversion");

                    Version = null;

                    ProductCode = metadata.GetProperty(MsiPropertyName.ProductCode);
                    if (String.IsNullOrEmpty(ProductCode))
                        throw new Exception($"MSI installer \"{FileName}\" has no ProductCode property. This property is REQUIRED according to the official documentation: https://docs.microsoft.com/en-us/windows/desktop/msi/productcode");
                }
            });
        }
    }
}
