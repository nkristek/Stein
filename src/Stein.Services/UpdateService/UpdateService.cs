using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stein.Common.UpdateService;
using Stein.Services.InstallerFiles.GitHub;
using Stein.Utility;

namespace Stein.Services.UpdateService
{
    /// <inheritdoc cref="IUpdateService" />
    public class UpdateService
        : Disposable, IUpdateService
    {
        private readonly Version _currentVersion;

        private readonly string _repository;

        /// <inheritdoc />
        public UpdateService(Version currentVersion, string repository)
        {
            _currentVersion = currentVersion ?? throw new ArgumentNullException(nameof(currentVersion));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com/repos/"),
            DefaultRequestHeaders = { { "User-Agent", "nkristek/Stein" } }
        };

        /// <inheritdoc />
        public async Task<IUpdateResult> IsUpdateAvailable(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_repository + "/releases", cancellationToken);
            response.EnsureSuccessStatusCode();

            var releases = await ParseResponse(response, cancellationToken);
            var updateResult = new UpdateResult
            {
                CurrentVersion = _currentVersion,
                NewestVersion = _currentVersion
            };

            foreach (var release in releases.Where(r => !r.IsDraft && !r.IsPreRelease && !String.IsNullOrEmpty(r.TagName)))
            {
                var lowerCaseTagName = release.TagName.ToLower();
                var versionString = lowerCaseTagName.StartsWith("v") ? lowerCaseTagName.Substring(1) : lowerCaseTagName;
                if (Version.TryParse(versionString, out var version) && updateResult.NewestVersion <= version)
                {
                    updateResult.NewestVersion = version;
                    updateResult.NewestVersionUri = String.IsNullOrEmpty(release.HtmlUrl) ? null : new Uri(release.HtmlUrl);
                    updateResult.ReleaseTag = release.TagName;
                    updateResult.UpdateAssets = release.Assets
                        .Where(a => !String.IsNullOrEmpty(a.BrowserDownloadUrl) && !String.IsNullOrEmpty(a.Name))
                        .Select(a => new UpdateAsset
                    {
                        DownloadUri = new Uri(a.BrowserDownloadUrl),
                        FileName = a.Name
                    });
                }
            }

            return updateResult;
        }

        private static async Task<IEnumerable<GitHubRelease>> ParseResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<List<GitHubRelease>>(content), cancellationToken);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            _httpClient.Dispose();
        }
    }
}
