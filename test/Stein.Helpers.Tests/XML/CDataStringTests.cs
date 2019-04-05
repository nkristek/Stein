using System;
using System.IO;
using System.Xml.Serialization;
using Stein.Helpers.XML;
using Xunit;

namespace Stein.Helpers.Tests.XML
{
    public class CDataStringTests
    {
        [Serializable]
        public class TestClass
        {
            [XmlElement]
            public CDataString String;

            private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(TestClass), typeof(TestClass).GetNestedTypes());

            public string ToXml()
            {
                using (var stringWriter = new StringWriter())
                {
                    XmlSerializer.Serialize(stringWriter, this);
                    return stringWriter.ToString();
                }
            }

            public static TestClass CreateFromXml(string xmlString)
            {
                using (var reader = new StringReader(xmlString))
                    return XmlSerializer.Deserialize(reader) as TestClass;
            }
        }

        [Fact]
        public void ToXml()
        {
            var instance = new TestClass
            {
                String = "test"
            };
            Assert.Contains("<![CDATA[test]]>", instance.ToXml());
        }

        [Fact]
        public void CreateFromXml()
        {
            var xmlData = "<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><String><![CDATA[test]]></String></TestClass>";
            var test = TestClass.CreateFromXml(xmlData);
            Assert.Equal("test", test.String);
        }
    }
}
