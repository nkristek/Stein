using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.ConfigurationTypes
{
    [Serializable]
    [XmlType("Date")]
    public class DateTimeXml
    {
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
            get
            {
                return Date?.ToString();
            }

            set
            {
                if (DateTime.TryParse(value, out DateTime dateTime))
                    Date = dateTime;
                else
                    Date = null;
            }
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

        public static DateTime TrimDateTimeToXmlAccuracy(DateTime dateTime)
        {
            if (DateTime.TryParse(dateTime.ToString(), out DateTime parsedDateTime))
                return parsedDateTime;
            else
                return DateTime.MinValue;
        }
    }
}
