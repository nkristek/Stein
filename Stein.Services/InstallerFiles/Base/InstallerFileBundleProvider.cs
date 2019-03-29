using System;
using System.Collections.Generic;
using System.Linq;
using Stein.Services.InstallerFiles.Disk;
using Stein.Services.InstallerFiles.GitHub;

namespace Stein.Services.InstallerFiles.Base
{
    public static class InstallerFileBundleProvider
    {
        private static readonly IReadOnlyList<Type> AllProviderTypes = GetAllAvailableProviderTypes();

        /// <summary>
        /// Get all types that implement <see cref="IInstallerFileBundleProvider"/>.
        /// </summary>
        /// <returns>A list of all types that implement <see cref="IInstallerFileBundleProvider"/>.</returns>
        private static IReadOnlyList<Type> GetAllAvailableProviderTypes()
        {
            // TODO
            //return AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(assembly => assembly.GetTypes())
            //    .Where(type => !type.IsAbstract && !type.IsInterface && typeof(IInstallerFileBundleProvider).IsAssignableFrom(type))
            //    .ToList();
            return new List<Type>
            {
                typeof(DiskInstallerFileBundleProvider),
                typeof(GitHubInstallerFileBundleProvider)
            };
        }

        public static IInstallerFileBundleProvider Create(Configuration.v1.InstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var provider = AllProviderTypes
                .Select(Activator.CreateInstance).OfType<IInstallerFileBundleProvider>()
                .FirstOrDefault(p => p.Type == configuration.Type);
            if (provider == null)
                throw new Exception($"The installer file provider set in the configuration file is unknown: {configuration.Type}");

            provider.Configurator.LoadConfiguration(configuration.ToDictionary());
            return provider;
        }

        public static IInstallerFileBundleProvider Create(Configuration.v2.InstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var provider = AllProviderTypes
                .Select(Activator.CreateInstance).OfType<IInstallerFileBundleProvider>()
                .FirstOrDefault(p => p.Type == configuration.Type);
            if (provider == null)
                throw new Exception($"The installer file provider set in the configuration file is unknown: {configuration.Type}");

            provider.Configurator.LoadConfiguration(configuration.ToDictionary());
            return provider;
        }
    }
}
