using System;
using System.Xml.Serialization;

namespace Stein.Utility.XML
{
    /// <summary>
    /// Encodes and decodes a <see cref="string"/> as CDATA in XML serialization.
    /// https://stackoverflow.com/a/19832309
    /// 
    /// <example> 
    /// <code>
    /// [XmlElement]
    /// public CDataString String;
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class CDataString : IXmlSerializable, IComparable<CDataString>
    {
        private string _value;

        public CDataString() : this(string.Empty)
        {
        }

        public CDataString(string value)
        {
            _value = value;
        }

        public static implicit operator CDataString(string value)
        {
            return new CDataString(value);
        }
        
        public static implicit operator string(CDataString cdata)
        {
            return cdata?._value;
        }

        public int CompareTo(CDataString other)
        {
            return String.Compare(ToString(), other?.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return _value;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            _value = reader.ReadElementString();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteCData(_value);
        }
    }
}
