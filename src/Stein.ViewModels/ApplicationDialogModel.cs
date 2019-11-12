using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class ApplicationDialogModel
        : DialogModel
    {
        public ApplicationDialogModel()
        {
            Validate();
            PropertyChanged += ApplicationDialogModel_PropertyChanged;
        }

        private void Validate()
        {
            ValidateName();
            ValidateKeepNewestInstallationLogsString();
            ValidateSelectedProvider();
        }

        private void ApplicationDialogModel_PropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null || String.IsNullOrEmpty(e.PropertyName))
            {
                Validate();
                return;
            }

            if (e.PropertyName == nameof(Name))
                ValidateName();

            if (e.PropertyName == nameof(KeepNewestInstallationLogsString) || e.PropertyName == nameof(AutomaticallyDeleteInstallationLogs))
                ValidateKeepNewestInstallationLogsString();

            if (e.PropertyName == nameof(SelectedProvider))
                ValidateSelectedProvider();
        }

        private Guid _entityId;
        
        public Guid EntityId
        {
            get => _entityId;
            set => SetProperty(ref _entityId, value);
        }

        private string? _name;
        
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private void ValidateName()
        {
            var validationErrors = new List<string>();
            if (String.IsNullOrEmpty(Name))
                validationErrors.Add(Strings.NameEmpty);
            SetErrors(validationErrors, nameof(Name));
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

        private string? _keepNewestInstallationLogsString;
        
        public string? KeepNewestInstallationLogsString
        {
            get => _keepNewestInstallationLogsString;
            set => SetProperty(ref _keepNewestInstallationLogsString, value);
        }

        private void ValidateKeepNewestInstallationLogsString()
        {
            if (!AutomaticallyDeleteInstallationLogs)
            {
                SetErrors(null, nameof(KeepNewestInstallationLogsString));
                return;
            }

            var validationErrors = new List<string>();
            if (!int.TryParse(KeepNewestInstallationLogsString, out var parsedValue))
                validationErrors.Add(Strings.NaN);
            else if (parsedValue < 1)
                validationErrors.Add(Strings.NumberShouldBeGreaterThanZero);
            SetErrors(validationErrors, nameof(KeepNewestInstallationLogsString));
        }

        [PropertySource(nameof(KeepNewestInstallationLogsString))]
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

        public ObservableCollection<InstallerFileBundleProviderViewModel> AvailableProviders { get; } = new ObservableCollection<InstallerFileBundleProviderViewModel>();

        private InstallerFileBundleProviderViewModel? _selectedProvider;
        
        public InstallerFileBundleProviderViewModel? SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                if (SetProperty(ref _selectedProvider, value, out var oldValue))
                {
                    if (oldValue != null)
                    {
                        oldValue.PropertyChanging -= SelectedProviderOnPropertyChanging;
                        oldValue.PropertyChanged -= SelectedProviderOnPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanging += SelectedProviderOnPropertyChanging;
                        value.PropertyChanged += SelectedProviderOnPropertyChanged;
                    }
                }
            }
        }

        private void ValidateSelectedProvider()
        {
            var validationErrors = new List<string>();
            if (SelectedProvider == null)
                validationErrors.Add(Strings.NoProvider);
            else if (!SelectedProvider.IsValid)
                validationErrors.Add(Strings.DialogInputNotValid);
            SetErrors(validationErrors, nameof(SelectedProvider));
        }

        private void SelectedProviderOnPropertyChanging(object? sender, PropertyChangingEventArgs? e)
        {
            NotifyPropertyChanging(nameof(SelectedProvider));
        }

        private void SelectedProviderOnPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            NotifyPropertyChanged(nameof(SelectedProvider));
        }

        private IViewModelCommand<ApplicationDialogModel>? _openLogFolderCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ApplicationDialogModel>? OpenLogFolderCommand
        {
            get => _openLogFolderCommand;
            set
            {
                if (SetProperty(ref _openLogFolderCommand, value, out var oldValue))
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
