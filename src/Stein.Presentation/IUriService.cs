using System;

namespace Stein.Presentation
{
    public interface IUriService
    {
        /// <summary>
        /// Opens the given <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">Uri to open.</param>
        void OpenUri(string uri);

        /// <summary>
        /// Opens the given <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">Uri to open.</param>
        void OpenUri(Uri uri);
    }
}
