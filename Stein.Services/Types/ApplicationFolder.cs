using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Stein.Helpers.XML;

namespace Stein.Services.Types
{
    [Serializable]
    public class ApplicationFolder
    {
        [XmlElement]
        public Guid Id;

        [XmlElement]
        public CDataString Name;

        [XmlElement]
        public CDataString Path;
        
        [XmlElement]
        public bool EnableSilentInstallation = true;
        
        [XmlElement]
        public bool DisableReboot = true;
        
        [XmlElement]
        public bool EnableInstallationLogging = true;
        
        [XmlElement]
        public bool AutomaticallyDeleteInstallationLogs = true;
        
        [XmlElement]
        public int KeepNewestInstallationLogs = 10;
        
        [XmlArray("SubFolders")]
        [XmlArrayItem("SubFolder")]
        public List<SubFolder> SubFolders = new List<SubFolder>();
    }
}
