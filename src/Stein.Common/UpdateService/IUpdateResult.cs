using System;
using System.Collections.Generic;

namespace Stein.Common.UpdateService
{
    /// <summary>
    /// The result of a check if an update is available.
    /// </summary>
    public interface IUpdateResult
    {
        /// <summary>
        /// If an update is available.
        /// </summary>
        bool IsUpdateAvailable { get; }

        /// <summary>
        /// The current application version.
        /// </summary>
        Version CurrentVersion { get; }

        /// <summary>
        /// The highest version found.
        /// </summary>
        Version NewestVersion { get; }

        /// <summary>
        /// The url to the release with the highest version found.
        /// </summary>
        Uri NewestVersionUri { get; }

        /// <summary>
        /// The tag of the found release.
        /// </summary>
        string ReleaseTag { get; }

        /// <summary>
        /// Assets of the update.
        /// </summary>
        IEnumerable<IUpdateAsset> UpdateAssets { get; }
    }
}
