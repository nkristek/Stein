using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Stein.Presentation;

namespace Stein.Common.Configuration.v1
{
    [Serializable]
    [XmlRoot(nameof(Configuration))]
    public class Configuration
        : IConfiguration
    {
        /// <inheritdoc />
        [XmlIgnore]
        public long FileVersion { get; } = 1;

        [XmlElement]
        public Theme SelectedTheme;

        [XmlArray, XmlArrayItem(typeof(Application), ElementName = nameof(Application))]
        public List<Application> Applications = new List<Application>();
    }
}
