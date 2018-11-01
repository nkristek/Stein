using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Stein.Helpers.XML;

namespace Stein.Services.Types
{
    [Serializable]
    public class SubFolder
    {
        [XmlElement]
        public CDataString Name;

        [XmlElement]
        public CDataString Path;

        [XmlArray("Installers")]
        [XmlArrayItem("Installer")]
        public List<InstallerFile> InstallerFiles = new List<InstallerFile>();

        [XmlArray("SubFolders")]
        [XmlArrayItem("SubFolder")]
        public List<SubFolder> SubFolders = new List<SubFolder>();
    }
}
