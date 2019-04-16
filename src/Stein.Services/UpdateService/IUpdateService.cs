using System.Threading;
using System.Threading.Tasks;

namespace Stein.Services.UpdateService
{
    public interface IUpdateService
    {
        /// <summary>
        /// Checks if an update is available.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>If an update is available.</returns>
        Task<UpdateResult> IsUpdateAvailable(CancellationToken cancellationToken = default);
    }
}
