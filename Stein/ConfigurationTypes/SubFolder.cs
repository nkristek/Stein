using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace nkristek.Stein.ConfigurationTypes
{
    [Serializable]
    public class SubFolder
    {
        [XmlElement("Name")]
        public string Name;

        [XmlElement("Path")]
        public string Path;

        [XmlArray("Installers")]
        [XmlArrayItem("Installer")]
        public List<InstallerFile> InstallerFiles = new List<InstallerFile>();

        [XmlArray("SubFolders")]
        [XmlArrayItem("SubFolder")]
        public List<SubFolder> SubFolders = new List<SubFolder>();
    }
}
