namespace Stein.Services.InstallService.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// If the operation should be performed without UI.
    /// <para />
    /// Consider adding the <see cref="T:Stein.Services.Arguments.DisableRebootArgument" /> as well, otherwise the computer might suddenly restart.
    /// </summary>
    public class QuietArgument
        : IOperationArgument
    {
        /// <inheritdoc />
        public string Value => "/QN";
    }
}
