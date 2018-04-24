using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace Stein.Types
{
    /// <summary>
    /// Helper class to serialize/deserialize DateTime to/from XML
    /// Can be used like:
    /// [XmlElement("Created")]
    /// public DateTimeXml CreatedXml;
    /// </summary>
    [Serializable]
    [XmlType("Date")]
    public class DateTimeXml
    {
        // cant use default parameter null in other constructor because XML Serialization can't handle that
        public DateTimeXml() { }

        public DateTimeXml(DateTime dateTime)
        {
            Date = dateTime;
        }

        [XmlIgnore]
        public DateTime? Date { get; set; }

        [XmlText]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Value
        {
            get => Serialize(Date ?? DateTime.MinValue);
            set => Date = Deserialize(value);
        }

        public static implicit operator DateTime(DateTimeXml dateTimeXml)
        {
            return dateTimeXml.Date ?? DateTime.MinValue;
        }

        public static implicit operator DateTimeXml(DateTime dateTime)
        {
            return new DateTimeXml(dateTime);
        }

        public override string ToString()
        {
            return Value;
        }

        private static string Serialize(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds().ToString();
        }

        private static DateTime? Deserialize(string value)
        {
            if (!long.TryParse(value, out var valueAsLong))
                return null;

            return DateTimeOffset.FromUnixTimeMilliseconds(valueAsLong).UtcDateTime;
        }

        public static DateTime TrimDateTimeToXmlAccuracy(DateTime dateTime)
        {
            var serializedDateTime = Serialize(dateTime);
            return Deserialize(serializedDateTime) ?? DateTime.MinValue;
        }
    }
}
