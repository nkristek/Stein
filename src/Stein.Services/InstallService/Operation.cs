using System;
using System.Collections.Generic;
using System.Linq;

namespace Stein.Services.InstallService
{
    public class Operation
        : IOperation
    {
        /// <inheritdoc />
        public string Context { get; set; }

        /// <inheritdoc />
        public OperationType Type { get; set; }

        /// <inheritdoc />
        public IEnumerable<IOperationArgument> Arguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation" /> class with a context and type.
        /// </summary>
        /// <param name="context">The context of the operation. This can be the file path of the installer or the ProductCode to operate on.</param>
        /// <param name="type">The type of the operation.</param>
        /// <param name="arguments">A list of arguments for the operation.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="context"/> is <c>null</c> or empty.</exception>
        public Operation(string context, OperationType type, params IOperationArgument[] arguments)
        {
            if (String.IsNullOrEmpty(context))
                throw new ArgumentNullException(nameof(context));

            Context = context;
            Type = type;
            Arguments = arguments ?? Enumerable.Empty<IOperationArgument>();
        }
    }
}
