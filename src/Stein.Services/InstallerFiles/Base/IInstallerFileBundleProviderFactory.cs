namespace Stein.Services.InstallerFiles.Base
{
    /// <summary>
    /// A factory for creating <see cref="IInstallerFileBundleProvider"/>.
    /// </summary>
    public interface IInstallerFileBundleProviderFactory
    {
        /// <summary>
        /// Create a <see cref="IInstallerFileBundleProvider"/> from the given <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration"><see cref="IInstallerFileBundleProviderConfiguration"/> to configure the <see cref="IInstallerFileBundleProvider"/>.</param>
        /// <returns>A <see cref="IInstallerFileBundleProvider"/> created from the given <paramref name="configuration"/>.</returns>
        IInstallerFileBundleProvider Create(IInstallerFileBundleProviderConfiguration configuration);
    }
}
