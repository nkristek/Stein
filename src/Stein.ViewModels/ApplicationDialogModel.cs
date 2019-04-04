using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NKristek.Smaragd.Validation;
using NKristek.Smaragd.ViewModels;
using Stein.Localizations;

namespace Stein.ViewModels
{
    public sealed class ApplicationDialogModel
        : DialogModel
    {
        public ApplicationDialogModel()
        {
            AddValidation(() => Name, new PredicateValidation<string>(value => !String.IsNullOrEmpty(value), Strings.NameEmpty));
            AddValidation(() => KeepNewestInstallationLogsString, new PredicateValidation<string>(value => int.TryParse(value, out _), Strings.NaN));
            AddValidation(() => KeepNewestInstallationLogsString, new PredicateValidation<string>(value => int.TryParse(value, out var parsedValue) && parsedValue >= 1, Strings.NumberShouldBeGreaterThanZero));
            AddValidation(() => SelectedProvider, new PredicateValidation<InstallerFileBundleProviderViewModel>(value => value != null, Strings.NoProvider));
            AddValidation(() => SelectedProvider, new PredicateValidation<InstallerFileBundleProviderViewModel>(value => value != null && value.IsValid, Strings.DialogInputNotValid));
        }
        
        private Guid _entityId;
        
        public Guid EntityId
        {
            get => _entityId;
            set => SetProperty(ref _entityId, value, out _);
        }

        private string _name;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }
        
        private bool _enableSilentInstallation;
        
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value, out _);
        }

        private bool _disableReboot;
        
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value, out _);
        }

        private bool _enableInstallationLogging;
        
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value, out _);
        }

        private bool _automaticallyDeleteInstallationLogs;
        
        public bool AutomaticallyDeleteInstallationLogs
        {
            get => _automaticallyDeleteInstallationLogs;
            set => SetProperty(ref _automaticallyDeleteInstallationLogs, value, out _);
        }

        private string _keepNewestInstallationLogsString;
        
        public string KeepNewestInstallationLogsString
        {
            get => _keepNewestInstallationLogsString;
            set => SetProperty(ref _keepNewestInstallationLogsString, value, out _);
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
            set => SetProperty(ref _filterDuplicateInstallers, value, out _);
        }

        public ObservableCollection<InstallerFileBundleProviderViewModel> AvailableProviders { get; } = new ObservableCollection<InstallerFileBundleProviderViewModel>();

        private InstallerFileBundleProviderViewModel _selectedProvider;
        
        public InstallerFileBundleProviderViewModel SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                if (SetProperty(ref _selectedProvider, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= SelectedProviderOnPropertyChanged;
                    if (value != null)
                        value.PropertyChanged += SelectedProviderOnPropertyChanged;
                }
            }
        }

        private void SelectedProviderOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedProvider));
        }
    }
}
