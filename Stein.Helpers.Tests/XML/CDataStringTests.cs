using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stein.Helpers.XML;

namespace Stein.Helpers.Tests.XML
{
    [TestClass]
    public class CDataStringTests
    {
        [Serializable]
        public class CDataStringTestClass
        {
            [XmlElement]
            public CDataString String;

            private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(CDataStringTestClass), typeof(CDataStringTestClass).GetNestedTypes());

            public string ToXml()
            {
                using (var stringWriter = new StringWriter())
                {
                    XmlSerializer.Serialize(stringWriter, this);
                    return stringWriter.ToString();
                }
            }

            public static CDataStringTestClass CreateFromXml(string xmlString)
            {
                using (var reader = new StringReader(xmlString))
                    return XmlSerializer.Deserialize(reader) as CDataStringTestClass;
            }
        }

        [TestMethod]
        public void TestCDataString()
        {
            var instance = new CDataStringTestClass
            {
                String = "test"
            };

            var serializedInstance = instance.ToXml();
            Assert.IsTrue(serializedInstance.Contains("<![CDATA[test]]>"), "The serialized output doesn't contain CDATA escaped content.");
            
            var deserializedInstance = CDataStringTestClass.CreateFromXml(serializedInstance);
            Assert.AreEqual(instance.String.ToString(), deserializedInstance.String.ToString());
        }
    }
}
