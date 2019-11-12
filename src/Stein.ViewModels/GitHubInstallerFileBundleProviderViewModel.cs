using System;
using System.Collections.Generic;
using Stein.Common.Configuration.v2;
using Stein.Localization;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class GitHubInstallerFileBundleProviderViewModel
        : InstallerFileBundleProviderViewModel
    {
        public GitHubInstallerFileBundleProviderViewModel()
        {
            Validate();
            PropertyChanged += GitHubInstallerFileBundleProviderViewModel_PropertyChanged;
        }
        private void Validate()
        {
            ValidateRepository();
        }

        private void GitHubInstallerFileBundleProviderViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs? e)
        {
            if (e == null || String.IsNullOrEmpty(e.PropertyName))
            {
                Validate();
                return;
            }

            if (e.PropertyName == nameof(Repository))
                ValidateRepository();
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
            var dictionary = new Dictionary<string, string?> { { nameof(Repository), Repository } };
            return new InstallerFileBundleProviderConfiguration(ProviderType, dictionary);
        }

        private string? _repository;
        
        public string? Repository
        {
            get => _repository;
            set
            {
                if (SetProperty(ref _repository, value))
                    ValidateRepository();
            }
        }

        private void ValidateRepository()
        {
            var validationErrors = new List<string>();
            if (String.IsNullOrEmpty(Repository))
                validationErrors.Add(Strings.RepositoryEmpty);
            else if (!new GitHubRepositoryPathValidation().Validate(Repository))
                validationErrors.Add(Strings.RepositoryPathInvalid);
            SetErrors(validationErrors, nameof(Repository));
        }
    }
}
