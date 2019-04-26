using System;
using System.Xml.Serialization;
using Stein.Utility.XML;

namespace Stein.Common.Configuration.v1
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
