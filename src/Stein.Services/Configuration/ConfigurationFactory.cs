using System;
using Stein.Common.Configuration;

namespace Stein.Services.Configuration
{
    public class ConfigurationFactory
        : IConfigurationFactory
    {
        /// <inheritdoc />
        public IConfiguration Create(long fileVersion)
        {
            switch (fileVersion)
            {
                case 0: return new Common.Configuration.v0.Configuration();
                case 1: return new Common.Configuration.v1.Configuration();
                case 2: return new Common.Configuration.v2.Configuration();
                default: throw new NotSupportedException($"File version {fileVersion} is not supported");
            }
        }
    }
}
