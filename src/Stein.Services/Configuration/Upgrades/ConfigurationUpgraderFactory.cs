using System.Collections.Generic;

namespace Stein.Services.Configuration.Upgrades
{
    public class ConfigurationUpgraderFactory
        : IConfigurationUpgraderFactory
    {
        /// <inheritdoc />
        public IEnumerable<IConfigurationUpgrader> CreateAll()
        {
            //return AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(assembly => assembly.GetTypes())
            //    .Where(type => !type.IsAbstract && !type.IsInterface && typeof(IConfigurationUpgrader).IsAssignableFrom(type))
            //    .Select(Activator.CreateInstance)
            //    .OfType<IConfigurationUpgrader>();

            yield return new ConfigurationUpgraderFrom0To1();
            yield return new ConfigurationUpgraderFrom1To2();
        }
    }
}
