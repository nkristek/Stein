using System;
using System.IO;
using System.Xml.Serialization;
using Stein.Utility.XML;
using Xunit;

namespace Stein.Utility.Tests.XML
{
    public class DateTimeXmlTests
    {
        [Serializable]
        public class TestClass
        {
            [XmlElement]
            public DateTimeXml Date;

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
                Date = new DateTime(2000, 1, 1, 1, 1, 1)
            };
            var t = instance.ToXml();
            Assert.Contains("630822852610000000", instance.ToXml());
        }

        [Fact]
        public void CreateFromXml()
        {
            var xmlData = "<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Date>630822852610000000</Date></TestClass>";
            var test = TestClass.CreateFromXml(xmlData);
            Assert.Equal(new DateTime(2000, 1, 1, 1, 1, 1), test.Date.Value);
        }
    }
}
