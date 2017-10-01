using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TempManager.Configuration
{
    [Serializable]
    public class SetupConfiguration
    {
        [XmlElement("Path")]
        public string Path;
    }
}
