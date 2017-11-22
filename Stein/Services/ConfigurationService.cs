using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stein.ConfigurationTypes;
using System.Collections.Generic;
using WpfBase.XmlTypes;

namespace Stein.Services
{
    public static class ConfigurationService
    {
        private static Configuration _Configuration;
        public static Configuration Configuration
        {
            get
            {
                if (_Configuration == null)
                    LoadConfigurationFromDisk();
                return _Configuration;
            }

            private set
            {
                _Configuration = value;
            }
        }

        public static string ConfigurationFolderPath
        {
            get
            {
                var configurationFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein");

                if (!Directory.Exists(configurationFolderPath))
                    Directory.CreateDirectory(configurationFolderPath);

                return configurationFolderPath;
            }
        }

        public static string ConfiguationPath
        {
            get
            {
                return Path.Combine(ConfigurationFolderPath, "Config.xml");
            }
        }

        public static void SyncApplicationFoldersWithDisk()
        {
            foreach (var applicationFolder in Configuration.ApplicationFolders)
                SyncApplicationFolderWithDisk(applicationFolder);
        }

        public static void SyncApplicationFolderWithDisk(ApplicationFolder applicationFolder)
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
                SyncSubFolderWithDisk(folder);
            }

            applicationFolder.SubFolders = applicationFolder.SubFolders.OrderBy(subFolder => subFolder.Name).ToList();
        }

        public static void SyncSubFolderWithDisk(SubFolder subFolder)
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

        public static async Task SyncApplicationFolderWithDiskAsync(ApplicationFolder applicationFolder)
        {
            await Task.Run(() =>
            {
                SyncApplicationFolderWithDisk(applicationFolder);
            });
        }

        public static async Task SyncApplicationFoldersWithDiskAsync()
        {
            await Task.Run(() =>
            {
                SyncApplicationFoldersWithDisk();
            });
        }

        public static void LoadConfigurationFromDisk()
        {
            Configuration = ReadConfiguration();
        }

        public static async Task LoadConfigurationFromDiskAsync()
        {
            Configuration = await ReadConfigurationAsync();
        }

        private static Configuration ReadConfiguration()
        {
            try
            {
                return Configuration.CreateFromFile(ConfiguationPath);
            }
            catch
            {
                return new Configuration();
            }
        }
        
        private static async Task<Configuration> ReadConfigurationAsync()
        {
            try
            {
                return await Configuration.CreateFromFileAsync(ConfiguationPath);
            }
            catch
            {
                return new Configuration();
            }
        }
        
        public static void SaveConfigurationToDisk()
        {
            try
            {
                Configuration.ToFile(ConfiguationPath);
            }
            catch (Exception exception)
            {
                throw new Exception("Saving the configuration file failed.", exception);
            }
        }

        public static async Task SaveConfigurationToDiskAsync()
        {
            try
            {
                await Configuration.ToFileAsync(ConfiguationPath);
            }
            catch (Exception exception)
            {
                throw new Exception("Saving the configuration file failed.", exception);
            }
        }

        public static SubFolder FindSubFolder(ApplicationFolder applicationFolder, string subFolderPath)
        {
            if (!subFolderPath.StartsWith(applicationFolder.Path))
                return null;

            var relativePath = subFolderPath.Substring(applicationFolder.Path.Length).Split('\\').Where(subString => !String.IsNullOrEmpty(subString));
            return FindSubFolder(applicationFolder, relativePath);
        }

        private static SubFolder FindSubFolder(ApplicationFolder applicationFolder, IEnumerable<string> relativePath)
        {
            var subFolderName = relativePath.FirstOrDefault();
            if (subFolderName == null)
                return null;

            var subFolder = applicationFolder.SubFolders.FirstOrDefault(folder => folder.Name == subFolderName);
            if (subFolder == null)
                return null;

            return FindSubFolder(subFolder, relativePath.Skip(1));
        }

        public static SubFolder FindSubFolder(SubFolder folder, string subFolderPath)
        {
            if (!subFolderPath.StartsWith(folder.Path))
                return null;

            var relativePath = subFolderPath.Substring(folder.Path.Length).Split('\\').Where(subString => !String.IsNullOrEmpty(subString));
            return FindSubFolder(folder, relativePath);
        }

        private static SubFolder FindSubFolder(SubFolder folder, IEnumerable<string> relativePath)
        {
            var subFolder = folder;

            foreach (var subFolderName in relativePath)
            {
                subFolder = subFolder.SubFolders.FirstOrDefault(f => f.Name == subFolderName);
                if (subFolder == null)
                    return null;
            }

            return subFolder;
        }

        public static InstallerFile FindInstallerFile(SubFolder folder, string installerFilePath)
        {
            if (!installerFilePath.StartsWith(folder.Path))
                return null;

            var relativePath = installerFilePath.Substring(folder.Path.Length).Split('\\').Where(subString => !String.IsNullOrEmpty(subString));
            return FindInstallerFile(folder, relativePath);
        }

        private static InstallerFile FindInstallerFile(SubFolder folder, IEnumerable<string> relativePath)
        {
            if (!relativePath.Any())
                return null;

            var parentFolderOfInstallerFile = FindSubFolder(folder, relativePath.Take(relativePath.Count() - 1));
            if (parentFolderOfInstallerFile == null)
                return null;

            var installerFileName = relativePath.LastOrDefault();
            return parentFolderOfInstallerFile.InstallerFiles.FirstOrDefault(i => Path.GetFileName(i.Path) == installerFileName);
        }
    }
}
