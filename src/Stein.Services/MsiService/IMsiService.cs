using System.Collections.Generic;
using Microsoft.Deployment.WindowsInstaller;

namespace Stein.Services.MsiService
{
    /// <summary>
    /// Defines methods to read metadata of MSI files.
    /// </summary>
    public interface IMsiService
    {
        /// <summary>
        /// Get the database of an MSI file.
        /// </summary>
        /// <param name="fileName">Path to the MSI file.</param>
        /// <returns>Database of the specified MSI file.</returns>
        Database GetMsiDatabase(string fileName);

        /// <summary>
        /// Get all properties of an MSI file.
        /// </summary>
        /// <param name="fileName">Path to the MSI file.</param>
        /// <returns>All properties of the MSI file.</returns>
        IDictionary<string, string> GetAllPropertiesFromMsi(string fileName);

        /// <summary>
        /// Get all properties contained in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <returns>All properties</returns>
        IDictionary<string, string> GetAllPropertiesFromMsiDatabase(Database database);

        /// <summary>
        /// Get a specific property from an MSI file.
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <param name="propertyName">Requested property</param>
        /// <returns>Property if exists, null otherwise</returns>
        string GetPropertyFromMsi(string fileName, MsiPropertyName propertyName);

        /// <summary>
        /// Get a specific property from the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database">Database of an MSI file.</param>
        /// <param name="propertyName">Name of the requested property.</param>
        /// <returns>Property if exists, <c>null</c> otherwise.</returns>
        string GetPropertyFromMsiDatabase(Database database, MsiPropertyName propertyName);
    }
}
