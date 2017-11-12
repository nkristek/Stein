using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Stein.ViewModels;
using WpfBase.ViewModels;
using Stein.Services;

namespace Stein.Configuration
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
                SaveConfigurationToDisk();
            }
        }

        public static void SyncApplicationFolderWithDisk(ApplicationFolder applicationFolder)
        {
            var subDirectorNames = Directory.GetDirectories(applicationFolder.Path);

            // remove all directories which don't exist on the file system anymore
            applicationFolder.SubFolders.RemoveAll(folder => !subDirectorNames.Any(fileName => fileName == folder.Path));

            foreach (var subDirectoryName in subDirectorNames)
            {
                var folder = applicationFolder.SubFolders.FirstOrDefault(sf => sf.Path == subDirectoryName);
                if (folder == null)
                {
                    folder = new SubFolder
                    {
                        Path = subDirectoryName,
                        Name = new DirectoryInfo(subDirectoryName).Name
                    };
                    applicationFolder.SubFolders.Add(folder);
                }
                SyncSubFolderWithDisk(folder);
            }
        }

        private static void SyncSubFolderWithDisk(SubFolder subFolder)
        {
            var fileNames = Directory.GetFiles(subFolder.Path, "*.msi");

            // remove all files which don't exist on the file system anymore
            subFolder.InstallerFiles.RemoveAll(i => !fileNames.Any(fileName => fileName == i.Path));

            foreach (var fileName in fileNames)
            {
                var creationDate = new FileInfo(fileName).CreationTime;

                var previouslySeenFile = subFolder.InstallerFiles.FirstOrDefault(i => i.Path == fileName);
                if (previouslySeenFile != null)
                {
                    // this is probably the same file when the creation date matches
                    if (previouslySeenFile.Created == creationDate)
                        continue;

                    // not the same file since the creation date is different
                    subFolder.InstallerFiles.Remove(previouslySeenFile);
                }

                var database = InstallService.GetMsiDatabase(fileName);
                var installerFile = new InstallerFile
                {
                    Name = InstallService.GetPropertyFromMsiDatabase(database, InstallService.MsiPropertyName.ProductName),
                    Path = fileName,
                    IsEnabled = true,
                    Created = creationDate,
                    Version = InstallService.GetVersionFromMsiDatabase(database),
                    Culture = InstallService.GetCultureTagFromMsiDatabase(database),
                    ProductCode = InstallService.GetProductCodeFromMsiDatabase(database)
                };
                subFolder.InstallerFiles.Add(installerFile);
            }

            var subDirectoryNames = Directory.GetDirectories(subFolder.Path);

            // remove all directories which don't exist on the file system anymore
            subFolder.SubFolders.RemoveAll(folder => !subDirectoryNames.Any(fileName => fileName == folder.Path));

            foreach (var subDirectoryName in subDirectoryNames)
            {
                var folder = subFolder.SubFolders.FirstOrDefault(sf => sf.Path == subDirectoryName);
                if (folder == null)
                {
                    folder = new SubFolder
                    {
                        Path = subDirectoryName,
                        Name = new DirectoryInfo(subDirectoryName).Name
                    };
                    subFolder.SubFolders.Add(folder);
                }
                SyncSubFolderWithDisk(folder);
            }
        }

        public static async Task SyncApplicationFolderWithDiskAsync(ApplicationFolder applicationFolder)
        {
            await Task.Run(() =>
            {
                SyncApplicationFolderWithDisk(applicationFolder);
            });
        }

        public static void LoadConfigurationFromDisk()
        {
            Configuration = ReadConfiguration();
        }

        private static Configuration ReadConfiguration()
        {
            try
            {
                return Configuration.CreateFromXmlFile(ConfiguationPath);
            }
            catch
            {
                return new Configuration();
            }
        }

        public static async Task LoadConfigurationFromDiskAsync()
        {
            Configuration = await ReadConfigurationAsync();
        }

        private static async Task<Configuration> ReadConfigurationAsync()
        {
            try
            {
                return await Configuration.CreateFromXmlFileAsync(ConfiguationPath);
            }
            catch
            {
                return new Configuration();
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

        public static void SaveConfigurationToDisk()
        {
            try
            {
                Configuration.ToXmlFile(ConfiguationPath);
            }
            catch (Exception createException)
            {
                throw new Exception("Saving the configuration file failed.", createException);
            }
        }

        public static async Task SaveConfigurationToDiskAsync()
        {
            try
            {
                await Configuration.ToXmlFileAsync(ConfiguationPath);
            }
            catch (Exception createException)
            {
                throw new Exception("Saving the configuration file failed.", createException);
            }
        }
    }
}
