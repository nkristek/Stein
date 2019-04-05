using System;
using System.Diagnostics;

namespace Stein.ViewModels.Services
{
    public class UriService
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
