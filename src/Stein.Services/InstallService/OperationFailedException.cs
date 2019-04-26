using System;
using Stein.Common.InstallService;

namespace Stein.Services.InstallService
{
    /// <inheritdoc />
    /// <summary>
    /// An exception that is thrown from the <see cref="IInstallService"/> when an <see cref="IOperation"/> failed to execute.
    /// </summary>
    [Serializable]
    public class OperationFailedException
        : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Stein.Services.InstallService.OperationFailedException" /> class with the <see cref="T:Stein.Common.InstallService.IOperation" /> which failed and the exit code of the operation.
        /// </summary>
        /// <param name="operation">Operation which failed.</param>
        /// <param name="processExitCode">The exit code of the operation.</param>
        public OperationFailedException(IOperation operation, int processExitCode)
        {
            Operation = operation;
            ProcessExitCode = processExitCode;
        }

        /// <summary>
        /// The operation which failed.
        /// </summary>
        public IOperation Operation { get; }

        /// <summary>
        /// The exit code of the operation.
        /// </summary>
        public int ProcessExitCode { get; }

        /// <inheritdoc />
        public override string Message => $"The process exited with code {ProcessExitCode} after performing \"{Operation.Type.ToString()}\" with context \"{Operation.Context}\".";
    }
}
