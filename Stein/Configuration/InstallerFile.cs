using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stein.Configuration
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

        [XmlElement("Version", typeof(VersionXml))]
        public Version Version;

        [XmlElement("Culture")]
        public string Culture;

        [XmlElement("ProductCode")]
        public string ProductCode;

        [XmlElement("Created", typeof(DateXml))]
        public Date CreatedXml;

        [XmlIgnore]
        public DateTime Created
        {
            get
            {
                return CreatedXml.DateTime;
            }

            set
            {
                CreatedXml = new Date(value);
            }
        }
    }
}
