using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

            try
            {
                Process.Start(uri);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    uri = uri.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {uri}") 
                    { 
                        CreateNoWindow = true 
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", uri);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", uri);
                }
                else
                {
                    throw;
                }
            }
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
