using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Types
{
    /// <summary>
    /// Helper class to serialize/deserialize Version to/from XML
    /// Can be used like:
    /// [XmlElement("Version", typeof(VersionXml))]
    /// public Version Version;
    /// </summary>
    [Serializable]
    [XmlType("Version")]
    public class VersionXml
    {
        // cant use default parameter null in other constructor because XML Serialization can't handle that
        public VersionXml() { }

        public VersionXml(Version version)
        {
            Version = version;
        }

        [XmlIgnore]
        public Version Version { get; set; }

        [XmlText]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Value
        {
            get => Version == null ? String.Empty : Version.ToString();
            set
            {
                Version.TryParse(value, out var temp);
                Version = temp;
            }
        }

        public static implicit operator Version(VersionXml versionXml)
        {
            return versionXml.Version;
        }

        public static implicit operator VersionXml(Version version)
        {
            return new VersionXml(version);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
