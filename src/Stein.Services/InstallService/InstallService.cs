using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stein.Localizations;
using Stein.Utility;

namespace Stein.Services.InstallService
{
    /// <inheritdoc />
    public class InstallService
        : IInstallService
    {
        /// <inheritdoc />
        public void Perform(params IOperation[] operations)
        {
            foreach (var operation in operations)
                ValidateOperation(operation);

            foreach (var operation in operations)
            {
                var contextArgument = GetContextArgument(operation);
                var process = StartProcess(contextArgument, operation.Arguments.Select(a => a.Value));
                process.WaitForExit();
                if (process.HasExited && process.ExitCode != 0)
                    throw new OperationFailedException(operation, process.ExitCode);
            }
        }
        
        /// <inheritdoc />
        public async Task PerformAsync(params IOperation[] operations)
        {
            foreach (var operation in operations)
                ValidateOperation(operation);

            foreach (var operation in operations)
            {
                var contextArgument = GetContextArgument(operation);
                var process = StartProcess(contextArgument, operation.Arguments.Select(a => a.Value));
                await process.WaitForExitAsync().ConfigureAwait(false);
                if (process.HasExited && process.ExitCode != 0)
                    throw new OperationFailedException(operation, process.ExitCode);
            }
        }

        private static void ValidateOperation(IOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            switch (operation.Type)
            {
                case OperationType.Install:
                case OperationType.Reinstall:
                    if (String.IsNullOrEmpty(operation.Context))
                        throw new ArgumentException(Strings.InstallerPathIsEmpty);
                    if (!File.Exists(operation.Context))
                        throw new FileNotFoundException(String.Format(Strings.InstallerNotFound, operation.Context), operation.Context);
                    break;
                case OperationType.Uninstall:
                    if (String.IsNullOrEmpty(operation.Context))
                        throw new ArgumentException(Strings.ProductCodeIsEmpty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation.Type));
            }
        }

        private static string GetContextArgument(IOperation operation)
        {
            switch (operation.Type)
            {
                case OperationType.Install:
                    return $"/I \"{operation.Context}\" ";
                case OperationType.Reinstall:
                    return $"/FAMUS \"{operation.Context}\"";
                case OperationType.Uninstall:
                    return $"/X {operation.Context}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation.Type));
            }
        }

        private static Process StartProcess(string contextArgument, IEnumerable<string> additionalArguments)
        {
            if (String.IsNullOrEmpty(contextArgument))
                throw new ArgumentNullException(nameof(contextArgument));

            if (additionalArguments == null)
                throw new ArgumentNullException(nameof(additionalArguments));
            
            var startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = String.Join(" ", contextArgument, String.Join(" ", additionalArguments)),
                UseShellExecute = false
            };

            return Process.Start(startInfo);
        }
    }
}
