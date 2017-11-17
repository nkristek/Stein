using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public static void SyncApplicationFoldersWithDisk()
        {
            foreach (var applicationFolder in Configuration.ApplicationFolders)
                SyncApplicationFolderWithDisk(applicationFolder);
        }

        public static void SyncApplicationFolderWithDisk(ApplicationFolder applicationFolder)
        {
            var subDirectories = Directory.GetDirectories(applicationFolder.Path).Select(directoryName => new DirectoryInfo(directoryName));

            // remove all directories which don't exist on the file system anymore
            applicationFolder.SubFolders.RemoveAll(folder => !subDirectories.Any(dir => dir.FullName == folder.Path));

            foreach (var subDirectory in subDirectories.OrderBy(dir => dir.CreationTime))
            {
                var folder = applicationFolder.SubFolders.FirstOrDefault(sf => sf.Path == subDirectory.FullName);
                if (folder == null)
                {
                    folder = new SubFolder
                    {
                        Path = subDirectory.FullName,
                        Name = subDirectory.Name
                    };
                    applicationFolder.SubFolders.Add(folder);
                }
                SyncSubFolderWithDisk(folder);
            }
        }

        private static void SyncSubFolderWithDisk(SubFolder subFolder)
        {
            var files = Directory.GetFiles(subFolder.Path, "*.msi").Select(fileName => new FileInfo(fileName));

            // remove all files which don't exist on the file system anymore
            subFolder.InstallerFiles.RemoveAll(i => !files.Any(file => file.FullName == i.Path));

            foreach (var file in files.OrderBy(file => file.Name))
            {
                var fileCreationTime = DateXml.TrimDateTimeToXmlAccuracy(file.CreationTime);
                var previouslySeenFile = subFolder.InstallerFiles.FirstOrDefault(i => i.Path == file.FullName);
                if (previouslySeenFile != null)
                {
                    // this is probably the same file when the creation date matches
                    if (previouslySeenFile.Created == fileCreationTime)
                        continue;

                    // not the same file since the creation date is different
                    subFolder.InstallerFiles.Remove(previouslySeenFile);
                }

                var database = InstallService.GetMsiDatabase(file.FullName);
                var installerFile = new InstallerFile
                {
                    Name = InstallService.GetPropertyFromMsiDatabase(database, InstallService.MsiPropertyName.ProductName),
                    Path = file.FullName,
                    IsEnabled = true,
                    Created = fileCreationTime,
                    Version = InstallService.GetVersionFromMsiDatabase(database),
                    Culture = InstallService.GetCultureTagFromMsiDatabase(database),
                    ProductCode = InstallService.GetProductCodeFromMsiDatabase(database)
                };
                subFolder.InstallerFiles.Add(installerFile);
            }
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
