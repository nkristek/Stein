﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using nkristek.MVVMBase.ViewModels;

namespace nkristek.Stein.ViewModels
{
    public class InstallerBundleViewModel
        : ViewModel
    {
        public InstallerBundleViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }

        private string _Name;
        /// <summary>
        /// The name of the folder of the installer bundle
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Path;
        /// <summary>
        /// The full path to the folder of the installer bundle
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { SetProperty(ref _Path, value); }
        }

        /// <summary>
        /// Gets the culture of the installers, if the Culture property is the same on all Installers, otherwise null
        /// </summary>
        [PropertySource(nameof(Installers))]
        public string Culture
        {
            get
            {
                if (!Installers.Any())
                    return null;
                var culture = Installers.FirstOrDefault().Culture;
                return Installers.All(i => i.Culture != null && i.Culture == culture) ? culture : null;
            }
        }

        /// <summary>
        /// List of installers in this installer bundle
        /// </summary>
        public ObservableCollection<InstallerViewModel> Installers { get; } = new ObservableCollection<InstallerViewModel>();

        /// <summary>
        /// Returns the newest creation time of all installers
        /// </summary>
        [PropertySource(nameof(Installers))]
        public DateTime? NewestInstallerCreationTime
        {
            get { return Installers.Select(i => i.Created).Max(); }
        }

        /// <summary>
        /// Creates a unique string to identify this InstallerBundleViewModel
        /// </summary>
        /// <returns>A unique string to identify this InstallerBundleViewModel</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Name);

            if (Culture != null)
            {
                builder.Append(" - ");
                builder.Append(Culture);
            }

            return builder.ToString();
        }
    }
}
