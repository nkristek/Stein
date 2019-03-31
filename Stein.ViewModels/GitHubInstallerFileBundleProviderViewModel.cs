using System;
using System.Collections.Generic;
using NKristek.Smaragd.Validation;
using Stein.Localizations;
using Stein.Services.Configuration.v2;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class GitHubInstallerFileBundleProviderViewModel
        : InstallerFileBundleProviderViewModel
    {
        public GitHubInstallerFileBundleProviderViewModel()
        {
            AddValidation(() => Repository, new PredicateValidation<string>(value => !String.IsNullOrEmpty(value), Strings.RepositoryEmpty));
            AddValidation(() => Repository, new GitHubRepositoryPathValidation(Strings.RepositoryPathInvalid));
        }

        /// <inheritdoc />
        public override string ProviderType => "GitHub";

        /// <inheritdoc />
        public override void LoadConfiguration(InstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.ProviderType != ProviderType)
                throw new ArgumentException($"Configuration with invalid type {configuration.ProviderType}, expected {ProviderType}.");

            if (configuration.Parameters.TryGetValue(nameof(Repository), out var repository))
                Repository = repository;
        }

        /// <inheritdoc />
        public override InstallerFileBundleProviderConfiguration CreateConfiguration()
        {
            var dictionary = new Dictionary<string, string> { { nameof(Repository), Repository } };
            return new InstallerFileBundleProviderConfiguration(ProviderType, dictionary);
        }

        private string _repository;
        
        public string Repository
        {
            get => _repository;
            set => SetProperty(ref _repository, value, out _);
        }
    }
}
