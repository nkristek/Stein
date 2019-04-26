using System;
using System.Collections.Generic;

namespace Stein.Common.MsiService
{
    /// <inheritdoc />
    /// <summary>
    /// Accessor for metadata of an MSI file.
    /// </summary>
    public interface IMsiMetadata
        : IDisposable
    {
        /// <summary>
        /// Get all properties of the MSI file.
        /// </summary>
        /// <returns>All properties of the MSI file.</returns>
        IDictionary<string, string> GetAllProperties();

        /// <summary>
        /// Get a specific property from the MSI file.
        /// </summary>
        /// <param name="propertyName">Requested property.</param>
        /// <returns>Value of the requested property if exists, <c>null</c> otherwise.</returns>
        string GetProperty(MsiPropertyName propertyName);
    }
}
