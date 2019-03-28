using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NKristek.Smaragd.Validation;
using Stein.Localizations;
using Stein.Services.Configuration.v1;

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
        public override string Type => "Disk";

        public string LocalizedName => Strings.Disk;

        /// <inheritdoc />
        public override void LoadConfiguration(InstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.Type != Type)
                throw new ArgumentException($"Configuration with invalid type {configuration.Type}, expected {Type}.");

            var pathItem = configuration.Items.FirstOrDefault(i => i.Key == "Path");
            if (pathItem != null)
                Path = pathItem.Value;
        }

        /// <inheritdoc />
        public override InstallerFileBundleProviderConfiguration CreateConfiguration()
        {
            var dictionary = new Dictionary<string, string> { { nameof(Path), Path } };
            return new InstallerFileBundleProviderConfiguration(Type, dictionary);
        }

        private string _path;

        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value, out _);
        }
    }
}
