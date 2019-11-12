using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public class UpdateDialogModel
        : DialogModel
    {
        private Version? _currentVersion;

        public Version? CurrentVersion
        {
            get => _currentVersion;
            set => SetProperty(ref _currentVersion, value);
        }

        private Version? _updateVersion;

        public Version? UpdateVersion
        {
            get => _updateVersion;
            set => SetProperty(ref _updateVersion, value);
        }

        private Uri? _updateUri;

        public Uri? UpdateUri
        {
            get => _updateUri;
            set => SetProperty(ref _updateUri, value);
        }

        private string? _releaseTag;

        public string? ReleaseTag
        {
            get => _releaseTag;
            set => SetProperty(ref _releaseTag, value);
        }

        public ObservableCollection<UpdateAssetViewModel> UpdateAssets { get; } = new ObservableCollection<UpdateAssetViewModel>();

        private bool _isUpdateDownloading;

        public bool IsUpdateDownloading
        {
            get => _isUpdateDownloading;
            set => SetProperty(ref _isUpdateDownloading, value);
        }

        private bool _isUpdateCancelled;

        public bool IsUpdateCancelled
        {
            get => _isUpdateCancelled;
            set => SetProperty(ref _isUpdateCancelled, value);
        }

        private IViewModelCommand<UpdateDialogModel>? _openUpdateUriCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<UpdateDialogModel>? OpenUpdateUriCommand
        {
            get => _openUpdateUriCommand;
            set
            {
                if (SetProperty(ref _openUpdateUriCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<UpdateDialogModel>? _installUpdateCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<UpdateDialogModel>? InstallUpdateCommand
        {
            get => _installUpdateCommand;
            set
            {
                if (SetProperty(ref _installUpdateCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }

        private IViewModelCommand<UpdateDialogModel>? _cancelUpdateCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<UpdateDialogModel>? CancelUpdateCommand
        {
            get => _cancelUpdateCommand;
            set
            {
                if (SetProperty(ref _cancelUpdateCommand, value, out var oldValue))
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
