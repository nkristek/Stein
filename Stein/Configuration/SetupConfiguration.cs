using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stein.Configuration
{
    [Serializable]
    public class SetupConfiguration
    {
        [XmlElement("Name")]
        public string Name;

        [XmlElement("Path")]
        public string Path;

        [XmlElement("EnableSilentInstallation")]
        public bool EnableSilentInstallation;
    }
}
