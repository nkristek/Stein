namespace Stein.Common.InstallService.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// If automatic reboot should be disabled.
    /// </summary>
    public class DisableRebootArgument
        : IOperationArgument
    {
        /// <inheritdoc />
        public string Value => "/norestart";
    }
}
