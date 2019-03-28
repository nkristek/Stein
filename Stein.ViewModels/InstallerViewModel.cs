using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NKristek.Smaragd.ViewModels;
using Stein.ViewModels.Types;

namespace Stein.ViewModels
{
    public sealed class InstallerViewModel
        : ViewModel
    {
        public InstallerViewModel()
        {
            foreach (var operation in GetAvailableOperations())
                AvailableOperations.Add(operation);
            PreferredOperation = AvailableOperations.FirstOrDefault();
        }

        private IEnumerable<InstallerOperation> GetAvailableOperations()
        {
            yield return InstallerOperation.DoNothing;
            yield return InstallerOperation.Install;
            yield return InstallerOperation.Uninstall;
        }

        private string _fileName;

        /// <summary>
        /// Name of the installer file
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value, out _);
        }

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
            set => SetProperty(ref _isInstalled, value, out _);
        }
        
        private DateTime _created;

        /// <summary>
        /// When the installer file was created
        /// </summary>
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value, out _);
        }

        private string _customOperationArguments;

        /// <summary>
        /// Additional arguments for custom operation.
        /// </summary>
        public string CustomOperationArguments
        {
            get => _customOperationArguments;
            set => SetProperty(ref _customOperationArguments, value, out _);
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
        
        private IInstallerFileProvider _installerFileProvider;

        /// <summary>
        /// The provider for the installer file.
        /// </summary>
        public IInstallerFileProvider InstallerFileProvider
        {
            get => _installerFileProvider;
            set => SetProperty(ref _installerFileProvider, value, out _);
        }
    }
}
