using System;
using System.Collections.Generic;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.InstallerFiles.GitHub
{
    public class GitHubInstallerFileBundleProviderConfigurator
        : IInstallerFileBundleProviderConfigurator
    {
        public string Repository { get; private set; }
        
        /// <inheritdoc />
        public bool Validate()
        {
            // TODO: validate with a regular expression like https://stackoverflow.com/q/2514859
            return !String.IsNullOrWhiteSpace(Repository) && Repository.Contains("/");
        }

        /// <inheritdoc />
        public IDictionary<string, string> SaveConfiguration()
        {
            return new Dictionary<string, string> { { nameof(Repository), Repository } };
        }

        /// <inheritdoc />
        public void LoadConfiguration(IDictionary<string, string> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            
            if (configuration.TryGetValue(nameof(Repository), out var repository))
                Repository = repository;
            else
                throw new ArgumentException("The given dictionary does not contain the repository name value.");
        }
    }
}