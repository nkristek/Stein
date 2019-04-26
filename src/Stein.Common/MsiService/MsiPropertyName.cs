namespace Stein.Common.MsiService
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa370905(v=vs.85).aspx
    /// </summary>
    public enum MsiPropertyName
    {
        /// <summary>
        /// Internet address or URL for technical support.
        /// </summary>
        ARPHELPLINK,

        /// <summary>
        /// Technical support phone numbers.
        /// </summary>
        ARPHELPTELEPHONE,

        /// <summary>
        /// String displayed by a message box that prompts for a disk.
        /// </summary>
        DiskPrompt,

        /// <summary>
        /// Set to 1 (one) if the current installation is running from a package created through an administrative installation.
        /// </summary>
        IsAdminPackage,

        /// <summary>
        /// Places units to the left of the number.
        /// </summary>
        LeftUnit,

        /// <summary>
        /// Name of the application manufacturer. (Required)
        /// </summary>
        Manufacturer,

        /// <summary>
        /// The installer sets this property to 1 (one) when the installation uses a media source, such as a CD-ROM.
        /// </summary>
        MediaSourceDir,

        /// <summary>
        /// The presence of this property indicates that a product code changing transform is registered to the product.
        /// </summary>
        MSIINSTANCEGUID,

        /// <summary>
        /// This property indicates the installation of a new instance of a product with instance transforms.
        /// </summary>
        MSINEWINSTANCE,

        /// <summary>
        /// The installer sets this property for installations that a Concurrent Installation action runs.
        /// </summary>
        ParentProductCode,

        /// <summary>
        /// String used as a template for the PIDKEY property.
        /// </summary>
        PIDTemplate,

        /// <summary>
        /// A unique identifier for a specific product release. (Required)
        /// </summary>
        ProductCode,

        /// <summary>
        /// Human readable name of an application. (Required)
        /// </summary>
        ProductName,

        /// <summary>
        /// Set to the installed state of a product.
        /// </summary>
        ProductState,

        /// <summary>
        /// String format of the product version as a numeric value. (Required)
        /// </summary>
        ProductVersion,

        /// <summary>
        /// A GUID that represents a related set of products.
        /// </summary>
        UpgradeCode,

        /// <summary>
        /// Numeric language identifier (LANGID) for the database. (REQUIRED)
        /// </summary>
        ProductLanguage,
    }
}
