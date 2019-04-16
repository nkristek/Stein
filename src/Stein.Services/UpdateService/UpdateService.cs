using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stein.Services.InstallerFiles.GitHub;
using Stein.Utility;

namespace Stein.Services.UpdateService
{
    public class UpdateService
        : Disposable, IUpdateService
    {
        private readonly Version _currentVersion;

        private readonly string _repository;

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
        public async Task<UpdateResult> IsUpdateAvailable(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_repository + "/releases", cancellationToken);
            response.EnsureSuccessStatusCode();

            var releases = await ParseResponse(response, cancellationToken);
            var updateResult = new UpdateResult
            {
                CurrentVersion = _currentVersion,
                NewestVersion = _currentVersion
            };

            foreach (var release in releases)
            {
                var tagName = release.TagName;
                if (String.IsNullOrEmpty(tagName))
                    continue;

                var lowerCaseTagName = tagName.ToLower();
                var versionString = lowerCaseTagName.StartsWith("v") ? lowerCaseTagName.Substring(1) : lowerCaseTagName;
                if (Version.TryParse(versionString, out var version) && updateResult.NewestVersion <= version)
                {
                    updateResult.NewestVersion = version;
                    updateResult.NewestVersionUri = String.IsNullOrEmpty(release.HtmlUrl) ? null : new Uri(release.HtmlUrl);
                    updateResult.UpdateAssets = release.Assets.Select(a => new UpdateAsset
                    {
                        DownloadUri = new Uri(a.BrowserDownloadUrl),
                        FileName = a.Name,
                        ReleaseTag = release.TagName
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
