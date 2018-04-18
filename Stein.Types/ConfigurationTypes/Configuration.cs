using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Stein.Types.ConfigurationTypes
{
    [Serializable]
    [XmlRoot("Configuration")]
    public class Configuration
    {
        [XmlArray("ApplicationFolders")]
        [XmlArrayItem("ApplicationFolder")]
        public List<ApplicationFolder> ApplicationFolders = new List<ApplicationFolder>();

        /// <summary>
        /// Serializes the configuration to XML
        /// </summary>
        /// <returns>XML serialized string</returns>
        public string ToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            using(var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, this);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Serializes the configuration to XML asynchronously
        /// </summary>
        /// <returns>Task which returns the XML serialized string</returns>
        public async Task<string> ToXmlAsync()
        {
            return await Task.Run(() => ToXml());
        }

        /// <summary>
        /// Serializes the configuration to XML and writes it to disk
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        public void ToFile(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            using (var writer = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Serializes the configuration to XML and writes it to disk asynchronously
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Task with serializes and saves the configuration to disk</returns>
        public async Task ToFileAsync(string filePath)
        {
            await Task.Run(() => ToFile(filePath));
        }

        /// <summary>
        /// Creates an Instance from an XML serialized string
        /// </summary>
        /// <param name="xmlString">XML serialized string of the configuration</param>
        /// <returns>Deserialized configuration</returns>
        public static Configuration CreateFromXml(string xmlString)
        {
            using (var reader = new StringReader(xmlString))
                return Deserialize(reader);
        }

        /// <summary>
        /// Creates an Instance from an XML serialized string asynchronously
        /// </summary>
        /// <param name="xmlString">XML serialized string of the configuration</param>
        /// <returns>Task which returns the deserialized configuration</returns>
        public static async Task<Configuration> CreateFromXmlAsync(string xmlString)
        {
            return await Task.Run(() => CreateFromXml(xmlString));
        }

        /// <summary>
        /// Creates an Instance from an XML serialized string
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Deserialized configuration</returns>
        public static Configuration CreateFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
                return Deserialize(reader);
        }

        /// <summary>
        /// Creates an Instance from an XML serialized string asynchronously
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Task which returns the deserialized configuration</returns>
        public static async Task<Configuration> CreateFromFileAsync(string filePath)
        {
            return await Task.Run(() => CreateFromFile(filePath));
        }

        /// <summary>
        /// Deserializes an XML serialized configuration
        /// </summary>
        /// <param name="reader">TextReader which provides the XML serialized configuration</param>
        /// <returns>Deserialized configuration</returns>
        private static Configuration Deserialize(TextReader reader)
        {
            return new XmlSerializer(typeof(Configuration)).Deserialize(reader) as Configuration;
        }
    }
}
