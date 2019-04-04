using System.Collections.Generic;

namespace Stein.Services.InstallService
{
    /// <summary>
    /// Defines an operation to perform on the <see cref="IInstallService"/>.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// The context of the operation. This can be the file path of the installer or the ProductCode to operate on.
        /// </summary>
        string Context { get; }

        /// <summary>
        /// The type of the operation.
        /// </summary>
        OperationType Type { get; }

        /// <summary>
        /// Arguments of the operation.
        /// </summary>
        IEnumerable<IOperationArgument> Arguments { get; }
    }
}
