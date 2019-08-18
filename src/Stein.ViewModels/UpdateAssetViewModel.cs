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
            set => SetProperty(ref _fileName, value);
        }

        private Uri _downloadUri;

        public Uri DownloadUri
        {
            get => _downloadUri;
            set => SetProperty(ref _downloadUri, value);
        }
    }
}
