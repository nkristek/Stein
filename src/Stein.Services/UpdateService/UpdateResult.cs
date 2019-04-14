using System;

namespace Stein.Services.UpdateService
{
    public struct UpdateResult
    {
        public bool IsUpdateAvailable => NewestVersion > CurrentVersion;

        public Version CurrentVersion;

        public Version NewestVersion;

        public Uri NewestVersionUri;
    }
}
