using System;

namespace Stein.Utility
{
    /// <summary>
    /// A collection of temporary file names. When disposed, they will be deleted.
    /// </summary>
    public interface ITempFileCollection
        : IDisposable
    {
        /// <summary>
        /// Add a file name to this collection.
        /// </summary>
        /// <param name="fileName">File name to add to the collection.</param>
        void AddFileName(string fileName);

        /// <summary>
        /// Create a unique file name in the folder, add it to the collection and return it.
        /// </summary>
        /// <param name="fileExtension">Optional file extension of the temporary file name to create.</param>
        /// <returns>A unique file name in the folder.</returns>
        string CreateUniqueFileName(string? fileExtension = null);
    }
}
