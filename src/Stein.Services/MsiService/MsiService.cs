using Stein.Common.MsiService;

namespace Stein.Services.MsiService
{
    /// <inheritdoc />
    public class MsiService
        : IMsiService
    {
        /// <inheritdoc />
        public IMsiMetadata GetMsiMetadata(string fileName)
        {
            return new MsiMetadata(fileName);
        }
    }
}
