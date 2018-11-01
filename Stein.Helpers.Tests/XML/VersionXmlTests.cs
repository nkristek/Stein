using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stein.Helpers.XML;

namespace Stein.Helpers.Tests.XML
{
    [TestClass]
    public class VersionXmlTests
    {
        [Serializable]
        public class VersionXmlTestClass
        {
            [XmlElement(Type = typeof(VersionXml))]
            public Version Version;

            private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(VersionXmlTestClass), typeof(VersionXmlTestClass).GetNestedTypes());

            public string ToXml()
            {
                using (var stringWriter = new StringWriter())
                {
                    XmlSerializer.Serialize(stringWriter, this);
                    return stringWriter.ToString();
                }
            }

            public static VersionXmlTestClass CreateFromXml(string xmlString)
            {
                using (var reader = new StringReader(xmlString))
                    return XmlSerializer.Deserialize(reader) as VersionXmlTestClass;
            }
        }

        [TestMethod]
        public void TestVersionXml()
        {
            var version = new Version(1, 2, 3, 4);
            var instance = new VersionXmlTestClass
            {
                Version = version
            };
            var serializedInstance = instance.ToXml();
            var deserializedInstance = VersionXmlTestClass.CreateFromXml(serializedInstance);
            Assert.AreEqual(version, deserializedInstance.Version);
        }
    }
}
