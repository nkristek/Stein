using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Utility.XML
{
    /// <summary>
    /// Encodes and decodes a <see cref="DateTime"/> in XML serialization.
    ///
    /// <example> 
    /// <code>
    /// [XmlElement]
    /// public DateTimeXml DateTime;
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    [XmlType("DateTime")]
    public class DateTimeXml
    {
        [XmlIgnore]
        public DateTime Value { get; set; }

        [XmlText]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string StringValue
        {
            get => Serialize(Value);
            set => Value = Deserialize(value) ?? DateTime.MinValue;
        }

        public DateTimeXml() : this(DateTime.MinValue)
        {
        }

        public DateTimeXml(DateTime dateTime)
        {
            Value = dateTime;
        }

        public static implicit operator DateTimeXml(DateTime dateTime)
        {
            return new DateTimeXml(dateTime);
        }

        public static implicit operator DateTime(DateTimeXml dateTimeXml)
        {
            return dateTimeXml?.Value ?? default;
        }
        
        public override string ToString()
        {
            return StringValue;
        }
        
        private static string Serialize(DateTime dateTime)
        {
            return dateTime.Ticks.ToString();
        }

        private static DateTime? Deserialize(string value)
        {
            if (!long.TryParse(value, out var valueAsLong))
                return null;

            return new DateTime(valueAsLong);
        }
    }
}
