using System.IO;
using Stein.Common.IOService;

namespace Stein.Services.IOService
{
    /// <inheritdoc />
    public class IOService
        : IIOService
    {
        /// <inheritdoc />
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <inheritdoc />
        public void CreateFile(string path)
        {
            File.Create(path);
        }

        /// <inheritdoc />
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <inheritdoc />
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <inheritdoc />
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <inheritdoc />
        public void DeleteDirectory(string path)
        {
            Directory.Delete(path);
        }

        /// <inheritdoc />
        public string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
