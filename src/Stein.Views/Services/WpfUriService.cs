using System;
using System.Diagnostics;
using Stein.Presentation;

namespace Stein.Views.Services
{
    /// <inheritdoc />
    public class WpfUriService
        : IUriService
    {
        /// <inheritdoc />
        public void OpenUri(string uri)
        {
            if (String.IsNullOrEmpty(uri))
                throw new ArgumentNullException(nameof(uri));

            Process.Start(new ProcessStartInfo(uri));
        }

        /// <inheritdoc />
        public void OpenUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            OpenUri(uri.AbsoluteUri);
        }
    }
}
