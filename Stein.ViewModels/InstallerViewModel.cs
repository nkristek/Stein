using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.ViewModels;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class InstallerViewModel
        : ViewModel
    {
        private string _path;

        /// <summary>
        /// FilePath of the installer
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value, out _);
        }

        /// <summary>
        /// Gets all available operations for this installer
        /// </summary>
        public ObservableCollection<InstallerOperation> AvailableOperations { get; } = new ObservableCollection<InstallerOperation>();

        private InstallerOperation _preferredOperation;

        /// <summary>
        /// If the installer is enabled by the user
        /// </summary>
        public InstallerOperation PreferredOperation
        {
            get => _preferredOperation;
            set
            {
                if (AvailableOperations.Contains(value))
                    SetProperty(ref _preferredOperation, value, out _);
            }
        }
        
        /// <summary>
        /// If the installer is disabled by the system (for example when it isn't installed)
        /// </summary>
        [PropertySource(nameof(IsInstalled))]
        public bool IsDisabled => IsInstalled == null;

        private string _name;

        /// <summary>
        /// Name of the installer (from the Msi-properties)
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private Version _version;

        /// <summary>
        /// Version of the installer (from the Msi-properties)
        /// </summary>
        public Version Version
        {
            get => _version;
            set => SetProperty(ref _version, value, out _);
        }

        private string _culture;

        /// <summary>
        /// Culture in IetfLanguageTag-format of the installer (from the Msi-properties)
        /// </summary>
        public string Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value, out _);
        }

        private string _productCode;

        /// <summary>
        /// ProductCode of the installer (from the Msi-properties)
        /// </summary>
        public string ProductCode
        {
            get => _productCode;
            set => SetProperty(ref _productCode, value, out _);
        }

        private bool? _isInstalled;

        /// <summary>
        /// If the installer is installed
        /// </summary>
        public bool? IsInstalled
        {
            get => _isInstalled;
            set
            {
                if (SetProperty(ref _isInstalled, value, out _))
                {
                    PreferredOperation = InstallerOperation.DoNothing;

                    AvailableOperations.Clear();
                    foreach (var operation in GetAvailableOperations())
                        AvailableOperations.Add(operation);
                }
            }
        }

        private IEnumerable<InstallerOperation> GetAvailableOperations()
        {
            yield return InstallerOperation.DoNothing;

            if (IsInstalled == true)
            {
                yield return InstallerOperation.Reinstall;
                yield return InstallerOperation.Uninstall;
            }
            else
            {
                yield return InstallerOperation.Install;
            }
        }

        private DateTime? _created;

        /// <summary>
        /// When the installer file was created
        /// </summary>
        public DateTime? Created
        {
            get => _created;
            set => SetProperty(ref _created, value, out _);
        }
    }
}
