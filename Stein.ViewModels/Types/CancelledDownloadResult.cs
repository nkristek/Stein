namespace Stein.ViewModels.Types
{
    public class CancelledDownloadResult
        : IDownloadResult
    {
        /// <inheritdoc />
        public DownloadResultState Result => DownloadResultState.Cancelled;
    }
}
