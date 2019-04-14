using System;
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
    }
}
