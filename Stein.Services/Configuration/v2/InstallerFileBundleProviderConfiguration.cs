using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Stein.Services.Configuration.v2
{
    [Serializable]
    public class InstallerFileBundleProviderConfiguration
    {
        [XmlElement]
        public string Type;

        [XmlArray, XmlArrayItem(typeof(InstallerFileBundleProviderConfigurationItem), ElementName = "item")]
        public List<InstallerFileBundleProviderConfigurationItem> Items = new List<InstallerFileBundleProviderConfigurationItem>();

        public InstallerFileBundleProviderConfiguration()
        {
        }

        public InstallerFileBundleProviderConfiguration(string type, IDictionary<string, string> configuration)
        {
            Type = type;
            Items.AddRange(configuration.Select(kvp => new InstallerFileBundleProviderConfigurationItem(kvp.Key, kvp.Value)));
        }

        public IDictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Items)
                dictionary[item.Key] = item.Value;
            return dictionary;
        }
    }
}
