using System;

namespace Stein.ViewModels.Types
{
    public class FailedDownloadResult
        : IDownloadResult
    {
        public FailedDownloadResult(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// The result of the download.
        /// </summary>
        public DownloadResultState Result => DownloadResultState.Failed;

        /// <summary>
        /// The exception that was thrown during the download.
        /// </summary>
        public Exception Exception { get; }
    }
}
