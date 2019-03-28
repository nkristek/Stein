using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.ViewModels.Types
{
    public class InstallerFileProvider
        : IInstallerFileProvider
    {
        private readonly Func<string, IProgress<double>, CancellationToken, Task> _saveFileAsync;

        public InstallerFileProvider(Func<string, IProgress<double>, CancellationToken, Task> saveFileAsync)
        {
            _saveFileAsync = saveFileAsync ?? throw new ArgumentNullException(nameof(saveFileAsync));
        }

        /// <inheritdoc />
        public async Task SaveFileAsync(string filePath, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            await _saveFileAsync(filePath, progress, cancellationToken);
        }
    }
}
