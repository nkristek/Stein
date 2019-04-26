using System;
using System.Collections.Generic;

namespace Stein.Common.InstallerFiles
{
    /// <summary>
    /// A bundle of all installer files of a specific release.
    /// </summary>
    public interface IInstallerFileBundle
    {
        /// <summary>
        /// The name of the installer file bundle.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The creation date of the installer file bundle.
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// The installer files included in this bundle.
        /// </summary>
        IEnumerable<IInstallerFile> InstallerFiles { get; }
    }
}
