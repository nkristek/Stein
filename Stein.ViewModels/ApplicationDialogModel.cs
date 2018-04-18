using System;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using Stein.ViewModels.Commands.ApplicationDialogModelCommands;

namespace Stein.ViewModels
{
    public class ApplicationDialogModel
        : DialogModel
    {
        public ApplicationDialogModel()
        {
            SelectFolderCommand = new SelectFolderCommand(this);
            OpenLogFolderCommand = new OpenLogFolderCommand(this);
        }

        public ViewModelCommand<ApplicationDialogModel> SelectFolderCommand { get; }

        public ViewModelCommand<ApplicationDialogModel> OpenLogFolderCommand { get; }

        private string _name;
        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _path;
        /// <summary>
        /// Path to the application folder
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        private Guid _folderId;
        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
        public Guid FolderId
        {
            get => _folderId;
            set => SetProperty(ref _folderId, value);
        }

        private bool _enableSilentInstallation;
        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
        public bool EnableSilentInstallation
        {
            get => _enableSilentInstallation;
            set => SetProperty(ref _enableSilentInstallation, value);
        }

        private bool _disableReboot;
        /// <summary>
        /// If the installers should be able to automatically reboot if necessary
        /// </summary>
        public bool DisableReboot
        {
            get => _disableReboot;
            set => SetProperty(ref _disableReboot, value);
        }

        private bool _enableInstallationLogging;
        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get => _enableInstallationLogging;
            set => SetProperty(ref _enableInstallationLogging, value);
        }
    }
}
