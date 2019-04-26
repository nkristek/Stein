using System;
using System.Xml.Serialization;
using Stein.Utility.XML;

namespace Stein.Common.Configuration.v0
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
    }
}
