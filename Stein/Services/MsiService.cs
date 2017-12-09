using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Deployment.WindowsInstaller;

namespace nkristek.Stein.Services
{
    public static class MsiService
    {
        /*
         * https://msdn.microsoft.com/en-us/library/windows/desktop/aa370905(v=vs.85).aspx
         */

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

        /// <summary>
        /// Gets the msi database of an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer file</param>
        /// <returns>Msi Database of the specified installer file</returns>
        public static Database GetMsiDatabase(string fileName)
        {
            try
            {
                return new Database(fileName, DatabaseOpenMode.ReadOnly);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all properties of an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <returns>All properties of the installer file</returns>
        public static Dictionary<string, string> GetAllPropertiesFromMsi(string fileName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetAllPropertiesFromMsiDatabase(database);
        }

        /// <summary>
        /// Gets all properties
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <returns>All properties</returns>
        public static Dictionary<string, string> GetAllPropertiesFromMsiDatabase(Database database)
        {
            try
            {
                var properties = new Dictionary<string, string>();

                using (var view = database.OpenView(database.Tables["Property"].SqlSelectString))
                {
                    view.Execute();
                    foreach (var record in view) using (record)
                        properties.Add(record.GetString("Property"), record.GetString("Value"));
                }

                return properties;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specific property from an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <param name="propertyName">Requested property</param>
        /// <returns>Property if exists, null otherwise</returns>
        public static string GetPropertyFromMsi(string fileName, MsiPropertyName propertyName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetPropertyFromMsiDatabase(database, propertyName);
        }

        /// <summary>
        /// Gets a specific property
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <param name="propertyName">Requested property</param>
        /// <returns>Property if exists, null otherwise</returns>
        public static string GetPropertyFromMsiDatabase(Database database, MsiPropertyName propertyName)
        {
            try
            {
                return database.ExecutePropertyQuery(propertyName.ToString());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the culture with IetfLanguageTag-format on an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <returns>Culture with IetfLanguageTag-format</returns>
        public static string GetCultureTagFromMsi(string fileName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetCultureTagFromMsiDatabase(database);
        }

        /// <summary>
        /// Gets the culture with IetfLanguageTag-format
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <returns>Culture with IetfLanguageTag-format</returns>
        public static string GetCultureTagFromMsiDatabase(Database database)
        {
            try
            {
                var cultureIdProperty = GetPropertyFromMsiDatabase(database, MsiPropertyName.ProductLanguage);
                return !String.IsNullOrEmpty(cultureIdProperty) ? new CultureInfo(int.Parse(cultureIdProperty)).IetfLanguageTag : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the version of an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <returns>Version of the installer file</returns>
        public static Version GetVersionFromMsi(string fileName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetVersionFromMsiDatabase(database);
        }

        /// <summary>
        /// Gets the version
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <returns>Version</returns>
        public static Version GetVersionFromMsiDatabase(Database database)
        {
            try
            {
                var versionProperty = GetPropertyFromMsiDatabase(database, MsiPropertyName.ProductVersion);
                return !String.IsNullOrEmpty(versionProperty) ? new Version(versionProperty) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
