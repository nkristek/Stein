using System;
using System.Collections.Generic;
using System.IO;

namespace Stein.Helpers
{
    /// <inheritdoc cref="ITempFileCollection" />
    public class TempFileCollection
            : Disposable, ITempFileCollection
    {
        private readonly string _folderPath;

        private readonly HashSet<string> _tempFileNames = new HashSet<string>();

        public TempFileCollection(string folderPath)
        {
            if (String.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException(nameof(folderPath));
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Directory \"{folderPath}\" does not exist.");

            _folderPath = folderPath;
        }

        /// <inheritdoc />
        public void AddFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (_tempFileNames.Contains(fileName))
                throw new ArgumentException($"File name \"{fileName}\" already exists in the temp file collection");

            _tempFileNames.Add(fileName);
        }

        /// <inheritdoc />
        public string CreateUniqueFileName(string fileExtension = null)
        {
            string fileName;
            do
            {
                fileName = Path.Combine(_folderPath, Path.GetFileName(Path.GetTempFileName()));
                if (!String.IsNullOrWhiteSpace(fileExtension))
                    fileName = Path.ChangeExtension(fileName, fileExtension);
            } while (_tempFileNames.Contains(fileName) && !File.Exists(fileName));

            _tempFileNames.Add(fileName);
            return fileName;
        }

        /// <inheritdoc />
        protected override void DisposeNativeResources()
        {
            foreach (var fileName in _tempFileNames)
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                    // ignored, file maybe already deleted
                }
            }
        }
    }
}
