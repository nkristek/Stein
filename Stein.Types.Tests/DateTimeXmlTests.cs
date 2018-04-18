using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Stein.Types.Tests
{
    [TestClass]
    public class DateTimeXmlTests
    {
        [Serializable]
        public class DateTimeXmlTestClass
        {
            [XmlElement("Date", typeof(DateTimeXml))]
            public DateTimeXml Date;

            private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(DateTimeXmlTestClass), typeof(DateTimeXmlTestClass).GetNestedTypes());

            public string ToXml()
            {
                using (var stringWriter = new StringWriter())
                {
                    XmlSerializer.Serialize(stringWriter, this);
                    return stringWriter.ToString();
                }
            }

            public static DateTimeXmlTestClass CreateFromXml(string xmlString)
            {
                using (var reader = new StringReader(xmlString))
                    return XmlSerializer.Deserialize(reader) as DateTimeXmlTestClass;
            }
        }

        [TestMethod]
        public void TestDateTimeXml()
        {
            var date = DateTimeXml.TrimDateTimeToXmlAccuracy(DateTime.Now);
            var instance = new DateTimeXmlTestClass
            {
                Date = date
            };
            var serializedInstance = instance.ToXml();
            var deserializedInstance = DateTimeXmlTestClass.CreateFromXml(serializedInstance);
            Assert.AreEqual(date, deserializedInstance.Date.Date);
        }
    }
}
