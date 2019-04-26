using System;

namespace Stein.Common.UpdateService
{
    /// <summary>
    /// An asset of a release.
    /// </summary>
    public interface IUpdateAsset
    {
        /// <summary>
        /// The <see cref="Uri"/> to download this asset from.
        /// </summary>
        Uri DownloadUri { get; }

        /// <summary>
        /// The name of the asset file.
        /// </summary>
        string FileName { get; }
    }
}
