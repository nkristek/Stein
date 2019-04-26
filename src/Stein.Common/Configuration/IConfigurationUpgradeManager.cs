namespace Stein.Common.Configuration
{
    /// <summary>
    /// Manages the upgrading of <see cref="IConfiguration"/>.
    /// </summary>
    public interface IConfigurationUpgradeManager
    {
        /// <summary>
        /// Upgrade the given <paramref name="configuration"/> to the latest file version.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/> to upgrade to the latest file version.</param>
        /// <param name="upgradedConfiguration">The upgraded <see cref="IConfiguration"/>.</param>
        /// <returns>If an upgrade has been performed.</returns>
        bool UpgradeToLatestFileVersion(IConfiguration configuration, out IConfiguration upgradedConfiguration);
    }
}
