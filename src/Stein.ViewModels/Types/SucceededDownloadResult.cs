namespace Stein.ViewModels.Types
{
    public class SucceededDownloadResult
        : IDownloadResult
    {
        public SucceededDownloadResult(string tempFileName)
        {
            TempFileName = tempFileName;
        }

        /// <inheritdoc />
        public DownloadResultState Result => DownloadResultState.CompletedSuccessfully;

        /// <summary>
        /// The name of the temporary file which was created.
        /// </summary>
        public string TempFileName { get; }
    }
}
