using System.Collections.Generic;

namespace Stein.Common.Configuration
{
    /// <summary>
    /// A factory for creating configuration upgraders.
    /// </summary>
    public interface IConfigurationUpgraderFactory
    {
        /// <summary>
        /// Create all configuration upgraders.
        /// </summary>
        /// <returns>All configuration upgraders.</returns>
        IEnumerable<IConfigurationUpgrader> CreateAll();
    }
}
