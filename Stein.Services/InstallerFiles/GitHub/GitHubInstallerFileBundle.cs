﻿using System;
using System.Collections.Generic;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.InstallerFiles.GitHub
{
    public class GitHubInstallerFileBundle
        : IInstallerFileBundle
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public DateTime Created { get; set; }

        /// <inheritdoc />
        public IEnumerable<IInstallerFile> InstallerFiles { get; set; }
    }
}
