using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stein.ConfigurationTypes
{
    public static class ApplicationFolderExtension
    {
        /// <summary>
        /// Finds a subfolder inside the ApplicationFolder by providing the full path to the subfolder
        /// </summary>
        /// <param name="applicationFolder">ApplicationFolder in which the subfolder exists</param>
        /// <param name="subFolderFullPath">Full path to the subfolder</param>
        /// <returns>The SubFolder if found, null otherwise</returns>
        public static SubFolder FindSubFolder(this ApplicationFolder applicationFolder, string subFolderFullPath)
        {
            if (!subFolderFullPath.StartsWith(applicationFolder.Path))
                return null;

            var relativePath = subFolderFullPath.Substring(applicationFolder.Path.Length).Split('\\').Where(subString => !String.IsNullOrEmpty(subString));
            return applicationFolder.FindSubFolder(relativePath);
        }

        /// <summary>
        /// Finds a subfolder inside the ApplicationFolder by providing the relative path to the subfolder
        /// </summary>
        /// <param name="applicationFolder">ApplicationFolder in which the subfolder exists</param>
        /// <param name="subFolderRelativePath">Relative path to the subfolder</param>
        /// <returns>The SubFolder if found, null otherwise</returns>
        public static SubFolder FindSubFolder(this ApplicationFolder applicationFolder, IEnumerable<string> subFolderRelativePath)
        {
            var subFolderName = subFolderRelativePath.FirstOrDefault();
            if (subFolderName == null)
                return null;

            var subFolder = applicationFolder.SubFolders.FirstOrDefault(folder => folder.Name == subFolderName);
            if (subFolder == null)
                return null;

            return subFolder.FindSubFolder(subFolderRelativePath.Skip(1));
        }

        /// <summary>
        /// Synchronize the ApplicationFolder with what exists on disk. It removes subfolders which aren't present anymore and adds new subfolders
        /// </summary>
        /// <param name="applicationFolder">The ApplicationFolder to synchronize</param>
        public static void SyncWithDisk(this ApplicationFolder applicationFolder)
        {
            var subDirectoriesOnDisk = Directory.GetDirectories(applicationFolder.Path).Select(directoryName => new DirectoryInfo(directoryName)).ToList();

            // remove all directories which don't exist on the file system anymore
            applicationFolder.SubFolders.RemoveAll(subFolder => !subDirectoriesOnDisk.Any(dir => dir.FullName == subFolder.Path));

            foreach (var subDirectoryOnDisk in subDirectoriesOnDisk)
            {
                var folder = applicationFolder.SubFolders.FirstOrDefault(subFolder => subFolder.Path == subDirectoryOnDisk.FullName);
                if (folder == null)
                {
                    folder = new SubFolder
                    {
                        Path = subDirectoryOnDisk.FullName,
                        Name = subDirectoryOnDisk.Name
                    };
                    applicationFolder.SubFolders.Add(folder);
                }
                folder.SyncWithDisk();
            }

            applicationFolder.SubFolders = applicationFolder.SubFolders.OrderBy(subFolder => subFolder.Name).ToList();
        }

        /// <summary>
        /// Synchronize the ApplicationFolder asynchronously with what exists on disk. It removes subfolders which aren't present anymore and adds new subfolders
        /// </summary>
        /// <param name="applicationFolder">The ApplicationFolder to synchronize</param>
        /// <returns>The asynchronous Task</returns>
        public static async Task SyncWithDiskAsync(this ApplicationFolder applicationFolder)
        {
            await Task.Run(() =>
            {
                applicationFolder.SyncWithDisk();
            });
        }
    }
}
