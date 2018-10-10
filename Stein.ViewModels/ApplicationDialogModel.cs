using System;
using NKristek.Smaragd.Commands;
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
            AddValidation(() => Path, new PredicateValidation<string>(value => !String.IsNullOrEmpty(value), Strings.PathEmpty));
            AddValidation(() => KeepNewestInstallationLogsString, new PredicateValidation<string>(value => int.TryParse(value, out _), Strings.NaN));
            AddValidation(() => KeepNewestInstallationLogsString, new PredicateValidation<string>(value => int.TryParse(value, out var parsedValue) && parsedValue >= 1, Strings.NumberShouldBeGreaterThanZero));
        }

        public ViewModelCommand<ApplicationDialogModel> SelectFolderCommand { get; set; }

        public ViewModelCommand<ApplicationDialogModel> OpenLogFolderCommand { get; set; }

        private string _name;

        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private string _path;

        /// <summary>
        /// Path to the application folder
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value, out _);
        }

        private Guid _folderId;

        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
        public Guid FolderId
        {
            get => _folderId;
            set => SetProperty(ref _folderId, value, out _);
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
            get => int.TryParse(KeepNewestInstallationLogsString, out var value) ? value : default(int);
            set => KeepNewestInstallationLogsString = value.ToString();
        }
    }
}
