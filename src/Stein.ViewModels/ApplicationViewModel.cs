using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class ApplicationViewModel
        : ViewModel
    {
        private Guid _entityId;
        
        public Guid EntityId
        {
            get => _entityId;
            set => SetProperty(ref _entityId, value);
        }

        private string _name;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        private bool _enableSilentInstallation;
        
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value);
        }

        private bool _disableReboot;
        
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value);
        }

        private bool _enableInstallationLogging;
        
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value);
        }

        private bool _automaticallyDeleteInstallationLogs;
        
        public bool AutomaticallyDeleteInstallationLogs
        {
            get => _automaticallyDeleteInstallationLogs;
            set => SetProperty(ref _automaticallyDeleteInstallationLogs, value);
        }

        private string _keepNewestInstallationLogsString;
        
        public string KeepNewestInstallationLogsString
        {
            get => _keepNewestInstallationLogsString;
            set => SetProperty(ref _keepNewestInstallationLogsString, value);
        }
        
        public int KeepNewestInstallationLogs
        {
            get => int.TryParse(KeepNewestInstallationLogsString, out var value) ? value : 0;
            set => KeepNewestInstallationLogsString = value.ToString();
        }

        private bool _filterDuplicateInstallers;
        
        public bool FilterDuplicateInstallers
        {
            get => _filterDuplicateInstallers;
            set => SetProperty(ref _filterDuplicateInstallers, value);
        }

        private string _providerType;

        public string ProviderType
        {
            get => _providerType;
            set => SetProperty(ref _providerType, value);
        }

        private string _providerLink;

        public string ProviderLink
        {
            get => _providerLink;
            set => SetProperty(ref _providerLink, value);
        }

        private InstallerBundleViewModel _selectedInstallerBundle;

        public InstallerBundleViewModel SelectedInstallerBundle
        {
            get => _selectedInstallerBundle;
            set => SetProperty(ref _selectedInstallerBundle, value);
        }

        public ObservableCollection<InstallerBundleViewModel> InstallerBundles { get; } = new ObservableCollection<InstallerBundleViewModel>();

        private IViewModelCommand<ApplicationViewModel> _editApplicationCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationViewModel> EditApplicationCommand
        {
            get => _editApplicationCommand;
            set
            {
                if (SetProperty(ref _editApplicationCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<ApplicationViewModel> _deleteApplicationCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationViewModel> DeleteApplicationCommand
        {
            get => _deleteApplicationCommand;
            set
            {
                if (SetProperty(ref _deleteApplicationCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<ApplicationViewModel> _installApplicationCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationViewModel> InstallApplicationCommand
        {
            get => _installApplicationCommand;
            set
            {
                if (SetProperty(ref _installApplicationCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<ApplicationViewModel> _uninstallApplicationCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationViewModel> UninstallApplicationCommand
        {
            get => _uninstallApplicationCommand;
            set
            {
                if (SetProperty(ref _uninstallApplicationCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<ApplicationViewModel> _customOperationApplicationCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationViewModel> CustomOperationApplicationCommand
        {
            get => _customOperationApplicationCommand;
            set
            {
                if (SetProperty(ref _customOperationApplicationCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<ApplicationViewModel> _openProviderLinkCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationViewModel> OpenProviderLinkCommand
        {
            get => _openProviderLinkCommand;
            set
            {
                if (SetProperty(ref _openProviderLinkCommand, value, out var oldValue))
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
