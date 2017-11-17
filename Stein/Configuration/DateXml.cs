using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Stein.Configuration
{
    [Serializable]
    [XmlType("Date")]
    public class DateXml
    {
        public DateXml() { }

        public DateXml(Date date)
        {
            Date = date;
        }

        [XmlIgnore]
        public Date Date { get; set; }

        [XmlText]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Value
        {
            get
            {
                return Date == null ? String.Empty : Date.DateTime.ToString();
            }

            set
            {
                if (DateTime.TryParse(value, out DateTime temp))
                    Date = new Date(temp);
                else
                    Date = null;
            }
        }

        public static implicit operator Date(DateXml dateXml)
        {
            return dateXml.Date;
        }

        public static implicit operator DateXml(Date date)
        {
            return new DateXml(date);
        }

        public override string ToString()
        {
            return Value;
        }

        public static DateTime TrimDateTimeToXmlAccuracy(DateTime dateTime)
        {
            var newDateTime = dateTime.Date;
            newDateTime.AddHours(dateTime.Hour);
            newDateTime.AddMinutes(dateTime.Minute);
            newDateTime.AddSeconds(dateTime.Second);
            return newDateTime;
        }
    }
}
