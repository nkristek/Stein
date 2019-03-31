using System.Collections.Generic;

namespace Stein.Services.Configuration.Upgrades
{
    public interface IConfigurationUpgraderFactory
    {
        IEnumerable<IConfigurationUpgrader> CreateAll();
    }
}
