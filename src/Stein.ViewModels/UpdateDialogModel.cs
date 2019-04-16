using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public class UpdateDialogModel
        : DialogModel
    {
        private Version _currentVersion;

        public Version CurrentVersion
        {
            get => _currentVersion;
            set => SetProperty(ref _currentVersion, value, out _);
        }

        private Version _updateVersion;

        public Version UpdateVersion
        {
            get => _updateVersion;
            set => SetProperty(ref _updateVersion, value, out _);
        }

        private Uri _updateUri;

        public Uri UpdateUri
        {
            get => _updateUri;
            set => SetProperty(ref _updateUri, value, out _);
        }

        public ObservableCollection<UpdateAssetViewModel> UpdateAssets { get; } = new ObservableCollection<UpdateAssetViewModel>();

        private bool _isUpdateDownloading;

        public bool IsUpdateDownloading
        {
            get => _isUpdateDownloading;
            set => SetProperty(ref _isUpdateDownloading, value, out _);
        }

        private bool _isUpdateCancelled;

        public bool IsUpdateCancelled
        {
            get => _isUpdateCancelled;
            set => SetProperty(ref _isUpdateCancelled, value, out _);
        }
    }
}
