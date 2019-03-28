using Stein.Services.Configuration.Upgrades;

namespace Stein.Services.Configuration
{
    /// <summary>
    /// Upgrades a configuration from one file version to another. 
    /// </summary>
    public interface IConfigurationUpgrader
    {
        /// <summary>
        /// The file version to upgrade from.
        /// </summary>
        long SourceFileVersion { get; }

        /// <summary>
        /// The file version to upgrade to.
        /// </summary>
        long TargetFileVersion { get; }

        /// <summary>
        /// Upgrades the given <paramref name="configuration"/> to the <see cref="TargetFileVersion"/>.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/> to upgrade.</param>
        /// <returns>The upgraded <see cref="IConfiguration"/>.</returns>
        /// <exception cref="InvalidFileVersionException">When the file version of the given <paramref name="configuration"/> is not supported by this <see cref="IConfigurationUpgrader"/>.</exception>
        IConfiguration Upgrade(IConfiguration configuration);
    }
}
