namespace Stein.Common.IOService
{
    /// <summary>
    /// Service for interacting with directories and files.
    /// </summary>
    public interface IIOService
    {
        /// <summary>
        /// Determines if a file at the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">Path of the file to check for.</param>
        /// <returns><c>true</c> if the file at the given <paramref name="path"/> exists, <c>false</c> otherwise.</returns>
        bool FileExists(string path);

        /// <summary>
        /// Creates a file at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path of the file to create.</param>
        void CreateFile(string path);

        /// <summary>
        /// Deletes a file at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path of the file to delete.</param>
        void DeleteFile(string path);

        /// <summary>
        /// Determines if a directory at the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">Path of the directory to check for.</param>
        /// <returns><c>true</c> if the directory at the given <paramref name="path"/> exists, <c>false</c> otherwise.</returns>
        bool DirectoryExists(string path);
        
        /// <summary>
        /// Creates a directory at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path of the directory to create.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Deletes a directory at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path of the directory to delete.</param>
        void DeleteDirectory(string path);

        /// <summary>
        /// Combines multiple <paramref name="paths"/>.
        /// </summary>
        /// <param name="paths">Paths to combine.</param>
        /// <returns>The combined <paramref name="paths"/>.</returns>
        string PathCombine(params string[] paths);
    }
}
