namespace Stein.Services.InstallService.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Specifies a path to a log file which should be created.
    /// </summary>
    public class LogFileArgument
        : IOperationArgument
    {
        private readonly string _logFilePath;

        /// <inheritdoc />
        public string Value => $"/L*V \"{_logFilePath}\"";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileArgument" /> class with a path to the log file which should be created.
        /// </summary>
        /// <param name="logFilePath">Path to the log file which should be created.</param>
        public LogFileArgument(string logFilePath)
        {
            _logFilePath = logFilePath;
        }
    }
}
