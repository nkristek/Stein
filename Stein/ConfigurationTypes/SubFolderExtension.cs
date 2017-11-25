using Stein.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WpfBase.XmlTypes;

namespace Stein.ConfigurationTypes
{
    public static class SubFolderExtension
    {
        /// <summary>
        /// Finds a subfolder inside the SubFolder by providing the full path to the subfolder
        /// </summary>
        /// <param name="folder">SubFolder in which the subfolder exists</param>
        /// <param name="subFolderFullPath">Full path to the subfolder</param>
        /// <returns>The SubFolder if found, null otherwise</returns>
        public static SubFolder FindSubFolder(this SubFolder folder, string subFolderFullPath)
        {
            if (!subFolderFullPath.StartsWith(folder.Path))
                return null;

            var relativePath = subFolderFullPath.Substring(folder.Path.Length).Split('\\').Where(subString => !String.IsNullOrEmpty(subString));
            return folder.FindSubFolder(relativePath);
        }

        /// <summary>
        /// Finds a subfolder inside the SubFolder by providing the relative path to the subfolder
        /// </summary>
        /// <param name="folder">SubFolder in which the subfolder exists</param>
        /// <param name="subFolderRelativePath">Relative path to the subfolder</param>
        /// <returns>The SubFolder if found, null otherwise</returns>
        public static SubFolder FindSubFolder(this SubFolder folder, IEnumerable<string> subFolderRelativePath)
        {
            var subFolder = folder;

            foreach (var subFolderName in subFolderRelativePath)
            {
                subFolder = subFolder.SubFolders.FirstOrDefault(f => f.Name == subFolderName);
                if (subFolder == null)
                    return null;
            }

            return subFolder;
        }

        /// <summary>
        /// Finds a installer file inside the SubFolder by providing the relative path to the file
        /// </summary>
        /// <param name="folder">SubFolder in which the installer file exists</param>
        /// <param name="installerFileFullPath">Full path to the installer file</param>
        /// <returns>The InstallerFile if found, null otherwise</returns>
        public static InstallerFile FindInstallerFile(this SubFolder folder, string installerFileFullPath)
        {
            if (!installerFileFullPath.StartsWith(folder.Path))
                return null;

            var relativePath = installerFileFullPath.Substring(folder.Path.Length).Split('\\').Where(subString => !String.IsNullOrEmpty(subString));
            return folder.FindInstallerFile(relativePath);
        }

        /// <summary>
        /// Finds a installer file inside the SubFolder by providing the relative path to the file
        /// </summary>
        /// <param name="folder">SubFolder in which the installer file exists</param>
        /// <param name="installerFileRelativePath">Relative path to the installer file</param>
        /// <returns>The InstallerFile if found, null otherwise</returns>
        public static InstallerFile FindInstallerFile(this SubFolder folder, IEnumerable<string> installerFileRelativePath)
        {
            if (!installerFileRelativePath.Any())
                return null;

            var parentFolderOfInstallerFile = folder.FindSubFolder(installerFileRelativePath.Take(installerFileRelativePath.Count() - 1));
            if (parentFolderOfInstallerFile == null)
                return null;

            var installerFileName = installerFileRelativePath.LastOrDefault();
            return parentFolderOfInstallerFile.InstallerFiles.FirstOrDefault(i => Path.GetFileName(i.Path) == installerFileName);
        }

        /// <summary>
        /// Finds all InstallerFiles inside the SubFolder hierarchy
        /// </summary>
        /// <param name="folder">SubFolder</param>
        /// <returns>List of found InstallerFiles</returns>
        public static IEnumerable<InstallerFile> FindAllInstallerFiles(this SubFolder folder)
        {
            foreach (var installerFile in folder.InstallerFiles)
                yield return installerFile;

            foreach (var subFolder in folder.SubFolders)
                foreach (var installerFile in subFolder.FindAllInstallerFiles())
                    yield return installerFile;
        }

        /// <summary>
        /// Synchronize the SubFolder with what exists on disk. It removes subfolders and installer files which aren't present anymore and adds new subfolders
        /// </summary>
        /// <param name="subFolder">The SubFolder to synchronize</param>
        public static void SyncWithDisk(this SubFolder subFolder)
        {
            var filesOnDisk = Directory.GetFiles(subFolder.Path, "*.msi").Select(fileName => new FileInfo(fileName)).ToList();

            // remove all files which don't exist on the file system anymore
            subFolder.InstallerFiles.RemoveAll(installerFile => !filesOnDisk.Any(file => file.FullName == installerFile.Path));

            foreach (var fileOnDisk in filesOnDisk)
            {
                var fileCreationTime = DateTimeXml.TrimDateTimeToXmlAccuracy(fileOnDisk.CreationTime);
                var existingInstallerFile = subFolder.InstallerFiles.FirstOrDefault(installerFile => installerFile.Path == fileOnDisk.FullName);
                if (existingInstallerFile != null)
                {
                    // this is probably the same file when the creation date matches
                    if (existingInstallerFile.Created == fileCreationTime)
                        continue;

                    LogService.LogInfo(String.Format("Creation date of previously seen installer file doesn't match. Will process installer file as new one. ({0})", existingInstallerFile.Path));

                    subFolder.InstallerFiles.Remove(existingInstallerFile);
                }

                using (var database = MsiService.GetMsiDatabase(fileOnDisk.FullName))
                {
                    subFolder.InstallerFiles.Add(new InstallerFile
                    {
                        Path = fileOnDisk.FullName,
                        IsEnabled = true,
                        Created = fileCreationTime,
                        Name = MsiService.GetPropertyFromMsiDatabase(database, MsiService.MsiPropertyName.ProductName),
                        Version = MsiService.GetVersionFromMsiDatabase(database),
                        Culture = MsiService.GetCultureTagFromMsiDatabase(database),
                        ProductCode = MsiService.GetPropertyFromMsiDatabase(database, MsiService.MsiPropertyName.ProductCode)
                    });
                }
            }

            subFolder.InstallerFiles = subFolder.InstallerFiles.OrderBy(installerFile => installerFile.Name).ToList();
        }
    }
}
