using System;
using System.Xml.Serialization;
using Stein.Utility.XML;

namespace Stein.Services.Configuration.v2
{
    [Serializable]
    public class InstallerFileBundleProviderConfigurationItem
    {
        [XmlElement]
        public string Key;

        [XmlElement]
        public CDataString Value;

        public InstallerFileBundleProviderConfigurationItem()
        {
        }

        public InstallerFileBundleProviderConfigurationItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
