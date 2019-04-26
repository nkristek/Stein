namespace Stein.Common.MsiService
{
    /// <summary>
    /// Defines methods to read metadata of MSI files.
    /// </summary>
    public interface IMsiService
    {
        /// <summary>
        /// Get the accessor to metadata of an MSI file.
        /// </summary>
        /// <param name="fileName">Path to the MSI file.</param>
        /// <returns>Accessor to metadata of the specified MSI file.</returns>
        IMsiMetadata GetMsiMetadata(string fileName);
    }
}
