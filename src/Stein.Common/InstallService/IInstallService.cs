using System.Threading.Tasks;

namespace Stein.Common.InstallService
{
    /// <summary>
    /// Defines methods for performing operations on installers (like installing, uninstalling, etc.).
    /// </summary>
    public interface IInstallService
    {
        /// <summary>
        /// Perform the given <paramref name="operations"/> in sequence.
        /// </summary>
        /// <param name="operations">Operations to perform in sequence.</param>
        void Perform(params IOperation[] operations);

        /// <summary>
        /// Asynchronously perform the given <paramref name="operations"/> in sequence.
        /// </summary>
        /// <param name="operations">Operations to perform in sequence.</param>
        /// <returns><see cref="Task"/> of the asynchronous execution.</returns>
        Task PerformAsync(params IOperation[] operations);
    }
}
