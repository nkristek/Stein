using System.Collections.Generic;
using Stein.Common.Configuration;

namespace Stein.Services.Configuration.Upgrades
{
    public class ConfigurationUpgraderFactory
        : IConfigurationUpgraderFactory
    {
        /// <inheritdoc />
        public IEnumerable<IConfigurationUpgrader> CreateAll()
        {
            yield return new ConfigurationUpgraderFrom0To1();
            yield return new ConfigurationUpgraderFrom1To2();
        }
    }
}
