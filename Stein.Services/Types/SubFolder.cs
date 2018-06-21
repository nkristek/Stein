using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Services.Types
{
    [Serializable]
    public class SubFolder
    {
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

        [XmlArray("Installers")]
        [XmlArrayItem("Installer")]
        public List<InstallerFile> InstallerFiles = new List<InstallerFile>();

        [XmlArray("SubFolders")]
        [XmlArrayItem("SubFolder")]
        public List<SubFolder> SubFolders = new List<SubFolder>();
    }
}
