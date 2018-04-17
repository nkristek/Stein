using Microsoft.Deployment.WindowsInstaller;
using Stein.Services.Types;
using System.Collections.Generic;

namespace Stein.Services
{
    public interface IMsiService
    {
        /// <summary>
        /// Gets the msi database of an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer file</param>
        /// <returns>Msi Database of the specified installer file</returns>
        Database GetMsiDatabase(string fileName);

        /// <summary>
        /// Gets all properties of an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <returns>All properties of the installer file</returns>
        Dictionary<string, string> GetAllPropertiesFromMsi(string fileName);

        /// <summary>
        /// Gets all properties
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <returns>All properties</returns>
        Dictionary<string, string> GetAllPropertiesFromMsiDatabase(Database database);

        /// <summary>
        /// Gets a specific property from an installer file
        /// </summary>
        /// <param name="fileName">Path to the installer</param>
        /// <param name="propertyName">Requested property</param>
        /// <returns>Property if exists, null otherwise</returns>
        string GetPropertyFromMsi(string fileName, MsiPropertyName propertyName);

        /// <summary>
        /// Gets a specific property
        /// </summary>
        /// <param name="database">Msi database of an installer file</param>
        /// <param name="propertyName">Requested property</param>
        /// <returns>Property if exists, null otherwise</returns>
        string GetPropertyFromMsiDatabase(Database database, MsiPropertyName propertyName);
    }
}
