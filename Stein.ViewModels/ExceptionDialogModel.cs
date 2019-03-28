using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class ExceptionDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        public override string Title => Exception?.TypeName;

        private ExceptionViewModel _exception;

        /// <summary>
        /// Contains information about an <see cref="Exception"/> that was thrown.
        /// </summary>
        public ExceptionViewModel Exception
        {
            get => _exception;
            set
            {
                if (SetProperty(ref _exception, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Parent = null;
                    if (value != null)
                        value.Parent = this;
                }
            }
        }
    }
}
