using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Stein.Presentation;

namespace Stein.Services.Configuration.v2
{
    [Serializable]
    [XmlRoot(nameof(Configuration))]
    public class Configuration
        : IConfiguration
    {
        /// <inheritdoc />
        [XmlIgnore]
        public long FileVersion { get; } = 2;

        [XmlElement]
        public Theme SelectedTheme;

        [XmlArray, XmlArrayItem(typeof(Application), ElementName = nameof(Application))]
        public List<Application> Applications = new List<Application>();
    }
}
