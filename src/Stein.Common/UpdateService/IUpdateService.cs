using System.Threading;
using System.Threading.Tasks;

namespace Stein.Common.UpdateService
{
    /// <summary>
    /// Service for checking if an update for the application is available.
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// Checks if an update is available.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns><c>true</c> if an update is available, <c>false</c> otherwise.</returns>
        Task<IUpdateResult> IsUpdateAvailable(CancellationToken cancellationToken = default);
    }
}
