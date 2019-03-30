using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.InstallerFiles.GitHub
{
    public class GitHubInstallerFileBundleProviderConfigurator
        : IInstallerFileBundleProviderConfigurator
    {
        public string Repository { get; private set; }

        private static readonly Regex RepositoryRegex = new Regex("^[a-z\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}\\/[^\\/]+$", RegexOptions.Compiled);
        
        /// <inheritdoc />
        public bool Validate()
        {
            return Repository != null && RepositoryRegex.IsMatch(Repository);
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