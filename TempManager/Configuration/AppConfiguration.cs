using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TempManager.Configuration
{
    [Serializable]
    [XmlRoot("AppConfiguration")]
    public class AppConfiguration
    {
        [XmlArray("Setups")]
        [XmlArrayItem("SetupConfiguration")]
        public List<SetupConfiguration> Setups = new List<SetupConfiguration>();

        public string ToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(AppConfiguration));

            using(var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, this);
                return stringWriter.ToString();
            }
        }

        public void ToXmlFile(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(AppConfiguration));

            using (var writer = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(writer, this);
            }
        }

        public static AppConfiguration CreateFromXml(string xmlString)
        {
            using (var reader = new StringReader(xmlString))
                return Deserialize(reader);
        }

        public static AppConfiguration CreateFromXmlFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
                return Deserialize(reader);
        }

        private static AppConfiguration Deserialize(TextReader reader)
        {
            return new XmlSerializer(typeof(AppConfiguration)).Deserialize(reader) as AppConfiguration;
        }
    }
}
