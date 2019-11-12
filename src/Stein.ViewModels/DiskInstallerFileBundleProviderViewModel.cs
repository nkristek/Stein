using System;
using System.Collections.Generic;
using System.IO;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Common.Configuration.v2;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class DiskInstallerFileBundleProviderViewModel
        : InstallerFileBundleProviderViewModel
    {
        public DiskInstallerFileBundleProviderViewModel()
        {
            Validate();
            PropertyChanged += DiskInstallerFileBundleProviderViewModel_PropertyChanged;
        }

        private void Validate()
        {
            ValidatePath();
        }

        private void DiskInstallerFileBundleProviderViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs? e)
        {
            if (e == null || String.IsNullOrEmpty(e.PropertyName))
            {
                Validate();
                return;
            }

            if (e.PropertyName == nameof(Path))
                ValidatePath();
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
            var dictionary = new Dictionary<string, string?> { { nameof(Path), Path } };
            return new InstallerFileBundleProviderConfiguration(ProviderType, dictionary);
        }

        private string? _path;
        
        public string? Path
        {
            get => _path;
            set
            {
                if (SetProperty(ref _path, value))
                    ValidatePath();
            }
        }

        private void ValidatePath()
        {
            var validationErrors = new List<string>();
            if (String.IsNullOrEmpty(Path))
                validationErrors.Add(Strings.PathEmpty);
            else if (!Directory.Exists(Path))
                validationErrors.Add(Strings.PathDoesNotExist);
            SetErrors(validationErrors, nameof(Path));
        }

        private IViewModelCommand<DiskInstallerFileBundleProviderViewModel>? _selectFolderCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<DiskInstallerFileBundleProviderViewModel>? SelectFolderCommand
        {
            get => _selectFolderCommand;
            set
            {
                if (SetProperty(ref _selectFolderCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }
    }
}
