using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Services.Types
{
    [Serializable]
    public class ApplicationFolder
    {
        [XmlElement("Id")]
        public Guid Id;

        [XmlIgnore]
        public string Name;

        [XmlElement("Name")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CData NameXml
        {
            get => Name;
            set => Name = value;
        }

        [XmlIgnore]
        public string Path;

        [XmlElement("Path")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CData PathXml
        {
            get => Path;
            set => Path = value;
        }

        [XmlElement("EnableSilentInstallation")]
        public bool EnableSilentInstallation;

        [XmlElement("DisableReboot")]
        public bool DisableReboot;

        [XmlElement("EnableInstallationLogging")]
        public bool EnableInstallationLogging;

        [XmlArray("SubFolders")]
        [XmlArrayItem("SubFolder")]
        public List<SubFolder> SubFolders = new List<SubFolder>();
    }
}
