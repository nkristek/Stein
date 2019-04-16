using System;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public class UpdateAssetViewModel
        : ViewModel
    {
        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value, out _);
        }

        private string _releaseTag;

        public string ReleaseTag
        {
            get => _releaseTag;
            set => SetProperty(ref _releaseTag, value, out _);
        }

        private Uri _downloadUri;

        public Uri DownloadUri
        {
            get => _downloadUri;
            set => SetProperty(ref _downloadUri, value, out _);
        }
    }
}
