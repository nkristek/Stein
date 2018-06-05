using System.Threading.Tasks;
using Stein.Services.Types;

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
        void LoadConfiguration();

        /// <summary>
        /// Loads the configuration file from the file system asynchronously
        /// </summary>
        /// <returns>Task which loads the configuration file from the file system</returns>
        Task LoadConfigurationAsync();

        /// <summary>
        /// Saves the configuration file to the file system
        /// </summary>
        void SaveConfiguration();

        /// <summary>
        /// Saves the configuration file to the file system asynchronously
        /// </summary>
        /// <returns>>Task which saves the configuration file to the file system</returns>
        Task SaveConfigurationAsync();
    }
}
