using System;
using System.Xml.Serialization;
using Stein.Helpers.XML;

namespace Stein.Services.Types
{
    [Serializable]
    public class InstallerFile
    {
        [XmlElement]
        public CDataString Name;

        [XmlElement]
        public CDataString Path;

        [XmlElement]
        public bool IsEnabled;
        
        [XmlElement(Type = typeof(VersionXml))]
        public Version Version;

        [XmlElement]
        public string Culture;

        [XmlElement]
        public string ProductCode;
        
        [XmlElement]
        public DateTimeXml Created;
    }
}
