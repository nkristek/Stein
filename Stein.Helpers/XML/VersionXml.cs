using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Helpers.XML
{
    /// <summary>
    /// Encodes and decodes a <see cref="Version"/> in XML serialization.
    /// https://stackoverflow.com/a/18962224
    /// 
    /// <example> 
    /// <code>
    /// [XmlElement(Type = typeof(VersionXml))]
    /// public Version Version;
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    [XmlType("Version")]
    public class VersionXml
    {
        [XmlIgnore]
        public Version Value { get; set; }

        [XmlText]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string StringValue
        {
            get => Value?.ToString();
            set => Value = Version.TryParse(value, out var temp) ? temp : null;
        }

        public VersionXml()
        {
        }

        public VersionXml(Version version)
        {
            Value = version;
        }
        
        public static implicit operator Version(VersionXml versionXml)
        {
            return versionXml.Value;
        }

        public static implicit operator VersionXml(Version version)
        {
            return new VersionXml(version);
        }

        public override string ToString()
        {
            return StringValue;
        }
    }
}
