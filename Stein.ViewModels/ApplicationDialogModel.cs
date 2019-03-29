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

        /// <summary>
        /// The Id of the associated entity.
        /// </summary>
        public Guid EntityId
        {
            get => _entityId;
            set => SetProperty(ref _entityId, value, out _);
        }

        private string _name;

        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }
        
        private bool _enableSilentInstallation;

        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value, out _);
        }

        private bool _disableReboot;

        /// <summary>
        /// If the installers should be able to automatically reboot if necessary
        /// </summary>
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value, out _);
        }

        private bool _enableInstallationLogging;

        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value, out _);
        }

        private bool _automaticallyDeleteInstallationLogs;

        /// <summary>
        /// If installation logs should be deleted automatically
        /// </summary>
        public bool AutomaticallyDeleteInstallationLogs
        {
            get => _automaticallyDeleteInstallationLogs;
            set => SetProperty(ref _automaticallyDeleteInstallationLogs, value, out _);
        }

        private string _keepNewestInstallationLogsString;

        /// <summary>
        /// How many installation logs should be kept. The oldest ones will be deleted first.
        /// </summary>
        public string KeepNewestInstallationLogsString
        {
            get => _keepNewestInstallationLogsString;
            set => SetProperty(ref _keepNewestInstallationLogsString, value, out _);
        }

        /// <summary>
        /// How many installation logs should be kept. The oldest ones will be deleted first.
        /// </summary>
        public int KeepNewestInstallationLogs
        {
            get => int.TryParse(KeepNewestInstallationLogsString, out var value) ? value : 0;
            set => KeepNewestInstallationLogsString = value.ToString();
        }

        /// <summary>
        /// Gets all available installer file provider.
        /// </summary>
        public ObservableCollection<InstallerFileBundleProviderViewModel> AvailableProviders { get; } = new ObservableCollection<InstallerFileBundleProviderViewModel>();

        private InstallerFileBundleProviderViewModel _selectedProvider;

        /// <summary>
        /// The selected installer file provider.
        /// </summary>
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

        private bool _filterDuplicateInstallers;

        /// <summary>
        /// If duplicate installers should be filtered while Install/Uninstall operation (Custom is not affected).
        /// </summary>
        public bool FilterDuplicateInstallers
        {
            get => _filterDuplicateInstallers;
            set => SetProperty(ref _filterDuplicateInstallers, value, out _);
        }
    }
}
