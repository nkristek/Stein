using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Services.Types
{
    [Serializable]
    public class InstallerFile
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
