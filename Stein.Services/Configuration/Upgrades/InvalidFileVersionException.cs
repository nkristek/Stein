using System;

namespace Stein.Services.Configuration.Upgrades
{
    /// <inheritdoc />
    /// <summary>
    /// This <see cref="T:System.Exception" /> is thrown when an <see cref="T:Stein.Services.Configuration.IConfigurationUpgrader" /> is given an <see cref="T:Stein.Services.Configuration.IConfiguration" /> with an invalid file version.
    /// </summary>
    public class InvalidFileVersionException
        : Exception
    {
        /// <summary>
        /// The expected source file version of the <see cref="IConfiguration"/>.
        /// </summary>
        public long ExpectedFileVersion { get; }

        /// <summary>
        /// The actual source file version of the <see cref="IConfiguration"/>.
        /// </summary>
        public long ActualFileVersion { get; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Stein.Services.Configuration.Upgrades.InvalidFileVersionException" /> class.
        /// </summary>
        /// <param name="expectedFileVersion">The expected source file version of the <see cref="T:Stein.Services.Configuration.IConfiguration" />.</param>
        /// <param name="actualFileVersion">The actual source file version of the <see cref="T:Stein.Services.Configuration.IConfiguration" />.</param>
        public InvalidFileVersionException(long expectedFileVersion, long actualFileVersion)
        {
            ExpectedFileVersion = expectedFileVersion;
            ActualFileVersion = actualFileVersion;
        }
    }
}
