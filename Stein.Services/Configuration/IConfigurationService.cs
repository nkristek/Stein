using System.Threading.Tasks;

namespace Stein.Services.Configuration
{
    /// <summary>
    /// Defines a configuration property and methods for loading and saving the configuration.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// The current configuration.
        /// </summary>
        v1.Configuration Configuration { get; }
        
        /// <summary>
        /// Load the configuration asynchronously.
        /// </summary>
        /// <returns>Asynchronous <see cref="Task"/> which loads the configuration.</returns>
        Task LoadConfigurationAsync();
        
        /// <summary>
        /// Save the configuration asynchronously.
        /// </summary>
        /// <returns>Asynchronous <see cref="Task"/> which saves the configuration.</returns>
        Task SaveConfigurationAsync();
    }
}
