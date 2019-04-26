using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Stein.Presentation;

namespace Stein.Common.Configuration.v0
{
    [Serializable]
    [XmlRoot(nameof(Configuration))]
    public class Configuration
        : IConfiguration
    {
        /// <inheritdoc />
        [XmlIgnore]
        public long FileVersion { get; }
        
        [XmlElement]
        public Theme SelectedTheme;

        [XmlArray, XmlArrayItem(typeof(ApplicationFolder), ElementName = nameof(ApplicationFolder))]
        public List<ApplicationFolder> ApplicationFolders = new List<ApplicationFolder>();
    }
}
