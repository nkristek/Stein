namespace Stein.Common.InstallService
{
    /// <summary>
    /// Argument of an <see cref="IOperation"/>.
    /// </summary>
    public interface IOperationArgument
    {
        /// <summary>
        /// The complete value of this argument which should be appended.
        /// </summary>
        string Value { get; }
    }
}
