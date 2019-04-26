using System;

namespace Stein.Common.Configuration
{
    /// <summary>
    /// A factory for creating configurations.
    /// </summary>
    public interface IConfigurationFactory
    {
        /// <summary>
        /// Create a <see cref="IConfiguration"/> with a specific <paramref name="fileVersion"/>.
        /// </summary>
        /// <param name="fileVersion">File version of the configuration.</param>
        /// <returns>A <see cref="IConfiguration"/> with the specified <paramref name="fileVersion"/>.</returns>
        /// <exception cref="NotSupportedException">If there isn't a known <see cref="IConfiguration"/> with the specified <paramref name="fileVersion"/>.</exception>
        IConfiguration Create(long fileVersion);
    }
}
