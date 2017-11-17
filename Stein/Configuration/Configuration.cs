using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stein.Configuration
{
    [Serializable]
    [XmlRoot("Configuration")]
    public class Configuration
    {
        [XmlArray("ApplicationFolders")]
        [XmlArrayItem("ApplicationFolder")]
        public List<ApplicationFolder> ApplicationFolders = new List<ApplicationFolder>();

        public string ToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            using(var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, this);
                return stringWriter.ToString();
            }
        }

        public async Task<string> ToXmlAsync()
        {
            return await Task.Run(() =>
            {
                return ToXml();
            });
        }

        public void ToXmlFile(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            using (var writer = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(writer, this);
            }
        }
        
        public async Task ToXmlFileAsync(string filePath)
        {
            await Task.Run(() =>
            {
                ToXmlFile(filePath);
            });
        }

        public static Configuration CreateFromXml(string xmlString)
        {
            using (var reader = new StringReader(xmlString))
                return Deserialize(reader);
        }

        public static async Task<Configuration> CreateFromXmlAsync(string xmlString)
        {
            return await Task.Run(() =>
            {
                return CreateFromXml(xmlString);
            });
        }

        public static Configuration CreateFromXmlFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
                return Deserialize(reader);
        }

        public static async Task<Configuration> CreateFromXmlFileAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                return CreateFromXmlFile(filePath);
            });
        }

        private static Configuration Deserialize(TextReader reader)
        {
            return new XmlSerializer(typeof(Configuration)).Deserialize(reader) as Configuration;
        }
    }
}
