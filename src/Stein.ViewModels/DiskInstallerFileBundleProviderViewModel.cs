using System;
using System.Collections.Generic;
using System.IO;
using NKristek.Smaragd.Validation;
using Stein.Common.Configuration.v2;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class DiskInstallerFileBundleProviderViewModel
        : InstallerFileBundleProviderViewModel
    {
        public DiskInstallerFileBundleProviderViewModel()
        {
            AddValidation(() => Path, new PredicateValidation<string>(value => !String.IsNullOrEmpty(value), Strings.PathEmpty));
            AddValidation(() => Path, new PredicateValidation<string>(Directory.Exists, Strings.PathDoesNotExist));
        }

        /// <inheritdoc />
        public override string ProviderType => "Disk";
        
        /// <inheritdoc />
        public override void LoadConfiguration(InstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.ProviderType != ProviderType)
                throw new ArgumentException($"Configuration with invalid type {configuration.ProviderType}, expected {ProviderType}.");

            if (configuration.Parameters.TryGetValue(nameof(Path), out var path))
                Path = path;
        }

        /// <inheritdoc />
        public override InstallerFileBundleProviderConfiguration CreateConfiguration()
        {
            var dictionary = new Dictionary<string, string> { { nameof(Path), Path } };
            return new InstallerFileBundleProviderConfiguration(ProviderType, dictionary);
        }

        private string _path;
        
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value, out _);
        }
    }
}
