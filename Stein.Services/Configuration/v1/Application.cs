using System;
using System.Xml.Serialization;
using Stein.Helpers.XML;
using Stein.Services.InstallerFiles.Base;

namespace Stein.Services.Configuration.v1
{
    [Serializable]
    public class Application
    {
        [XmlAttribute]
        public Guid Id;

        [XmlElement]
        public CDataString Name;
        
        [XmlElement]
        public bool EnableSilentInstallation = true;
        
        [XmlElement]
        public bool DisableReboot = true;
        
        [XmlElement]
        public bool EnableInstallationLogging = true;
        
        [XmlElement]
        public bool AutomaticallyDeleteInstallationLogs = true;
        
        [XmlElement]
        public int KeepNewestInstallationLogs = 10;
        
        [XmlIgnore]
        private InstallerFileBundleProviderConfiguration _configuration;

        [XmlElement]
        public InstallerFileBundleProviderConfiguration Configuration
        {
            get => _configuration;
            set
            {
                if (_configuration == value)
                    return;
                _configuration = value;
                if (_cachedProvider is IDisposable disposable)
                    disposable.Dispose();
                _cachedProvider = null;
            }
        }

        [XmlIgnore]
        private IInstallerFileBundleProvider _cachedProvider;

        [XmlIgnore]
        public IInstallerFileBundleProvider Provider => _cachedProvider ?? (_cachedProvider = InstallerFileBundleProvider.Create(Configuration));
    }
}
