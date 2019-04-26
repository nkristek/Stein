using System.Windows;
using Stein.Presentation;

namespace Stein.Views.Services
{
    /// <inheritdoc />
    public class WpfClipboardService
        : IClipboardService
    {
        /// <inheritdoc />
        public void SetText(string text)
        {
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
        }
    }
}
