using System;
using System.Collections.ObjectModel;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using nkristek.Stein.Commands.ApplicationDialogModelCommands;

namespace nkristek.Stein.ViewModels
{
    public class ApplicationDialogModel
        : ViewModel
    {
        public ApplicationDialogModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            SelectFolderCommand = new SelectFolderCommand(this);
            OpenLogFolderCommand = new OpenLogFolderCommand(this);
        }

        public ViewModelCommand<ApplicationDialogModel> SelectFolderCommand { get; private set; }

        public ViewModelCommand<ApplicationDialogModel> OpenLogFolderCommand { get; private set; }

        private string _Name;
        /// <summary>
        /// Name of the application folder
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Path;
        /// <summary>
        /// Path to the application folder
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { SetProperty(ref _Path, value); }
        }

        private Guid _FolderId;
        /// <summary>
        /// The Id of the ApplicationFolder in the configuration
        /// </summary>
        public Guid FolderId
        {
            get { return _FolderId; }
            set { SetProperty(ref _FolderId, value); }
        }

        private bool _EnableSilentInstallation;
        /// <summary>
        /// If the installations should proceed without UI
        /// </summary>
        public bool EnableSilentInstallation
        {
            get { return _EnableSilentInstallation; }
            set { SetProperty(ref _EnableSilentInstallation, value); }
        }

        private bool _DisableReboot;
        /// <summary>
        /// If the installers should be able to automatically reboot if necessary
        /// </summary>
        public bool DisableReboot
        {
            get { return _DisableReboot; }
            set { SetProperty(ref _DisableReboot, value); }
        }

        private bool _EnableInstallationLogging;
        /// <summary>
        /// If logging during installation should be enabled
        /// </summary>
        public bool EnableInstallationLogging
        {
            get { return _EnableInstallationLogging; }
            set { SetProperty(ref _EnableInstallationLogging, value); }
        }
    }
}
