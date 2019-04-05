using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stein.Helpers;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.InstallerFiles.GitHub
{
    public class GitHubInstallerFileBundleProvider
        : Disposable, IInstallerFileBundleProvider
    {
        public GitHubInstallerFileBundleProvider(IInstallerFileBundleProviderConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.ProviderType != ProviderType)
                throw new ArgumentException($"Invalid provider type (expected \"{ProviderType}\", got \"{configuration.ProviderType}\"", nameof(configuration));

            if (configuration.Parameters.TryGetValue(nameof(Repository), out var repository))
                Repository = repository;
        }

        /// <inheritdoc />
        public string ProviderType => "GitHub";

        /// <inheritdoc />
        public string ProviderLink => String.Concat("https://github.com/", Repository);

        /// <summary>
        /// The name of the GitHub repository including the user name (e.g. nkristek/Stein).
        /// </summary>
        public string Repository { get; }

        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com/repos/"),
            DefaultRequestHeaders = { { "User-Agent", "nkristek/Stein" } }
        };

        /// <inheritdoc />
        public async Task<IEnumerable<IInstallerFileBundle>> GetInstallerFileBundlesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(Repository + "/releases", cancellationToken);
            response.EnsureSuccessStatusCode();

            var releases = await ParseResponse(response, cancellationToken);

            var installerFileBundles = new List<IInstallerFileBundle>();

            foreach (var release in releases.OrderBy(r => r.CreatedAt))
            {
                var installerFiles = new List<IInstallerFile>();
                foreach (var asset in release.Assets.Where(a => a.Name.ToLower().EndsWith(".msi")))
                {
                    installerFiles.Add(new GitHubInstallerFile(asset.BrowserDownloadUrl)
                    {
                        FileName = asset.Name,
                        Created = asset.CreatedAt.ToLocalTime()
                    });
                }
                installerFileBundles.Add(new GitHubInstallerFileBundle
                {
                    Name = release.Name,
                    Created = release.CreatedAt.ToLocalTime(),
                    InstallerFiles = installerFiles
                });
            }

            return installerFileBundles;
        }

        private static async Task<IEnumerable<GitHubRelease>> ParseResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.Run(() => JsonConvert.DeserializeObject<List<GitHubRelease>>(content));
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            _httpClient.Dispose();
        }
    }
}
