using System;
using System.Collections.Generic;
using System.Linq;
using Stein.Common.UpdateService;

namespace Stein.Services.UpdateService
{
    /// <inheritdoc />
    public class UpdateResult
        : IUpdateResult
    {
        /// <inheritdoc/>
        public bool IsUpdateAvailable => NewestVersion > CurrentVersion;

        /// <inheritdoc/>
        public Version CurrentVersion { get; set; }

        /// <inheritdoc/>
        public Version NewestVersion { get; set; }

        /// <inheritdoc/>
        public Uri NewestVersionUri { get; set; }

        /// <inheritdoc/>
        public string ReleaseTag { get; set; }

        /// <inheritdoc/>
        public IEnumerable<IUpdateAsset> UpdateAssets { get; set; } = Enumerable.Empty<UpdateAsset>();
    }
}
