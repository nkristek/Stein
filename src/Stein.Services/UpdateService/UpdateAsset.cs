using System;

namespace Stein.Services.UpdateService
{
    public class UpdateAsset
    {
        public Uri DownloadUri { get; set; }

        public string FileName { get; set; }

        public string ReleaseTag { get; set; }
    }
}
