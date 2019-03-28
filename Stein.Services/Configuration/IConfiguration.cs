namespace Stein.Services.Configuration
{
    /// <summary>
    /// A configuration used to persistently store application settings.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// File version of this configuration.
        /// </summary>
        long FileVersion { get; }
    }
}
