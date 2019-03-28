using System;
using System.Collections.Generic;
using System.Linq;
using NKristek.Smaragd.Validation;
using Stein.Localizations;
using Stein.Services.Configuration.v1;

namespace Stein.ViewModels
{
    public sealed class GitHubInstallerFileBundleProviderViewModel
        : InstallerFileBundleProviderViewModel
    {
        public GitHubInstallerFileBundleProviderViewModel()
        {
            AddValidation(() => Repository, new PredicateValidation<string>(value => !String.IsNullOrEmpty(value), Strings.RepositoryEmpty));
            AddValidation(() => Repository, new PredicateValidation<string>(value => value != null && value.Contains("/"), Strings.RepositoryDoesNotContainSlash));
        }

        /// <inheritdoc />
        public override string Type => "GitHub";

        /// <inheritdoc />
        public override void LoadConfiguration(InstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.Type != Type)
                throw new ArgumentException($"Configuration with invalid type {configuration.Type}, expected {Type}.");

            var repositoryItem = configuration.Items.FirstOrDefault(i => i.Key == "Repository");
            if (repositoryItem != null)
                Repository = repositoryItem.Value;
        }

        /// <inheritdoc />
        public override InstallerFileBundleProviderConfiguration CreateConfiguration()
        {
            var dictionary = new Dictionary<string, string> { { nameof(Repository), Repository } };
            return new InstallerFileBundleProviderConfiguration(Type, dictionary);
        }

        private string _repository;

        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Repository
        {
            get => _repository;
            set => SetProperty(ref _repository, value, out _);
        }
    }
}
