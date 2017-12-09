using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace nkristek.Stein.ConfigurationTypes
{
    [Serializable]
    public class ApplicationFolder
    {
        [XmlElement("Id")]
        public Guid Id;

        [XmlElement("Name")]
        public string Name;

        [XmlElement("Path")]
        public string Path;

        [XmlElement("EnableSilentInstallation")]
        public bool EnableSilentInstallation;

        [XmlElement("EnableInstallationLogging")]
        public bool EnableInstallationLogging;

        [XmlArray("SubFolders")]
        [XmlArrayItem("SubFolder")]
        public List<SubFolder> SubFolders = new List<SubFolder>();
    }
}
