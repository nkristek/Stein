using Stein.Types.ConfigurationTypes;
using System.Threading.Tasks;

namespace Stein.Services
{
    public interface IConfigurationService
    {
        /// <summary>
        /// The current Configuration
        /// </summary>
        Configuration Configuration { get; }

        /// <summary>
        /// Loads the configuration file from the file system
        /// </summary>
        void LoadConfigurationFromDisk();

        /// <summary>
        /// Loads the configuration file from the file system asynchronously
        /// </summary>
        /// <returns>Task which loads the configuration file from the file system</returns>
        Task LoadConfigurationFromDiskAsync();

        /// <summary>
        /// Saves the configuration file to the file system
        /// </summary>
        void SaveConfigurationToDisk();

        /// <summary>
        /// Saves the configuration file to the file system asynchronously
        /// </summary>
        /// <returns>>Task which saves the configuration file to the file system</returns>
        Task SaveConfigurationToDiskAsync();
    }
}
