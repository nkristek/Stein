using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Types.ConfigurationTypes
{
    [Serializable]
    public class InstallerFile
    {
        [XmlElement("Name")]
        public string Name;

        [XmlElement("Path")]
        public string Path;

        [XmlElement("IsEnabled")]
        public bool IsEnabled;
        
        [XmlIgnore]
        public Version Version;

        [XmlElement("Version")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VersionXml VersionXml
        {
            get => Version;
            set => Version = value;
        }

        [XmlElement("Culture")]
        public string Culture;

        [XmlElement("ProductCode")]
        public string ProductCode;
        
        [XmlIgnore]
        public DateTime Created;

        [XmlElement("Created")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTimeXml CreatedXml
        {
            get => Created;
            set => Created = value;
        }
    }
}
