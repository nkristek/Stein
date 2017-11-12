using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                DateTime.TryParse(value, out DateTime temp);
                Date = new Date(temp);
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
    }
}
