namespace Stein.ViewModels.Types
{
    public interface IDownloadResult
    {
        /// <summary>
        /// The result of the download.
        /// </summary>
        DownloadResultState Result { get; }
    }
}
