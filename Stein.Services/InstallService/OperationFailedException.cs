using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stein.Services.InstallService
{
    public class OperationFailedException
        : Exception
    {
        public OperationFailedException(IOperation operation, int processExitCode)
        {
            Operation = operation;
            ProcessExitCode = processExitCode;
        }

        public IOperation Operation { get; }

        public int ProcessExitCode { get; }

        /// <inheritdoc />
        public override string Message => $"The process exited with code {ProcessExitCode} after performing \"{Operation.Type.ToString()}\" with context \"{Operation.Context}\".";
    }
}
