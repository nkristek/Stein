using System;
using System.Collections.Generic;
using System.Linq;

namespace Stein.Services.UpdateService
{
    public class UpdateResult
    {
        public bool IsUpdateAvailable => NewestVersion > CurrentVersion;

        public Version CurrentVersion { get; set; }

        public Version NewestVersion { get; set; }

        public Uri NewestVersionUri { get; set; }

        public IEnumerable<UpdateAsset> UpdateAssets { get; set; } = Enumerable.Empty<UpdateAsset>();
    }
}
