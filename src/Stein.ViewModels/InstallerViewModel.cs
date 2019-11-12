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
            SelectedOperation = AvailableOperations.FirstOrDefault();
        }

        private IEnumerable<InstallerOperation> GetAvailableOperations()
        {
            yield return InstallerOperation.DoNothing;
            yield return InstallerOperation.Install;
            yield return InstallerOperation.Uninstall;
        }

        private string? _fileName;
        
        public string? FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private string? _name;
        
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        private Version? _version;
        
        public Version? Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private string? _culture;
        
        public string? Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        private string? _productCode;
        
        public string? ProductCode
        {
            get => _productCode;
            set => SetProperty(ref _productCode, value);
        }

        private bool? _isInstalled;
        
        public bool? IsInstalled
        {
            get => _isInstalled;
            set => SetProperty(ref _isInstalled, value);
        }
        
        private DateTime _created;
        
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }

        private string? _customOperationArguments;
        
        public string? CustomOperationArguments
        {
            get => _customOperationArguments;
            set => SetProperty(ref _customOperationArguments, value);
        }
        
        private IInstallerFileProvider? _installerFileProvider;
        
        public IInstallerFileProvider? InstallerFileProvider
        {
            get => _installerFileProvider;
            set => SetProperty(ref _installerFileProvider, value);
        }

        public ObservableCollection<InstallerOperation> AvailableOperations { get; } = new ObservableCollection<InstallerOperation>();

        private InstallerOperation _selectedOperation;

        public InstallerOperation SelectedOperation
        {
            get => _selectedOperation;
            set => SetProperty(ref _selectedOperation, value);
        }
    }
}
