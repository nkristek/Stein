using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.Configuration.v2
{
    [Serializable]
    public class InstallerFileBundleProviderConfiguration
        : IInstallerFileBundleProviderConfiguration
    {
        public InstallerFileBundleProviderConfiguration()
        {
        }

        public InstallerFileBundleProviderConfiguration(string providerType, IDictionary<string, string> parameters)
        {
            if (String.IsNullOrEmpty(providerType))
                throw new ArgumentNullException(nameof(providerType));

            ProviderType = providerType;

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            Items.AddRange(parameters.Select(kvp => new InstallerFileBundleProviderConfigurationItem(kvp.Key, kvp.Value)));
        }

        [XmlElement("Type")]
        public string ProviderType { get; set; }
        
        [XmlIgnore]
        public IDictionary<string, string> Parameters
        {
            get
            {
                var dictionary = new Dictionary<string, string>();
                foreach (var item in Items)
                    dictionary[item.Key] = item.Value;
                return dictionary;
            }
        }

        [XmlArray, XmlArrayItem(typeof(InstallerFileBundleProviderConfigurationItem), ElementName = "item")]
        public List<InstallerFileBundleProviderConfigurationItem> Items = new List<InstallerFileBundleProviderConfigurationItem>();
    }
}
