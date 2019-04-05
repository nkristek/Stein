using System;
using System.Xml.Serialization;
using Stein.Utility.XML;

namespace Stein.Services.Configuration.v1
{
    [Serializable]
    public class Application
    {
        [XmlAttribute]
        public Guid Id;

        [XmlElement]
        public CDataString Name;
        
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
        
        [XmlElement]
        public InstallerFileBundleProviderConfiguration Configuration;
    }
}
