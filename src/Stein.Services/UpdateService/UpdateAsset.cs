using System;
using Stein.Common.UpdateService;

namespace Stein.Services.UpdateService
{
    /// <inheritdoc/>
    public class UpdateAsset
        : IUpdateAsset
    {
        /// <inheritdoc/>
        public Uri DownloadUri { get; set; }

        /// <inheritdoc/>
        public string FileName { get; set; }
    }
}
