namespace Stein.Presentation
{
    /// <summary>
    /// Service for interacting with the clipboard.
    /// </summary>
    public interface IClipboardService
    {
        /// <summary>
        /// Set text to the clipboard.
        /// </summary>
        /// <param name="text">Text to set to the clipboard.</param>
        void SetText(string text);

        /// <summary>
        /// Get text from the clipboard.
        /// </summary>
        /// <returns>Text from the clipboard.</returns>
        string GetText();
    }
}
