using NKristek.Smaragd.ViewModels;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class ExceptionDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        public override string Title => Exception is ExceptionViewModel exception ? $"{Strings.Error}: {exception.TypeName}" : Strings.Error;

        private ExceptionViewModel? _exception;
        
        public ExceptionViewModel? Exception
        {
            get => _exception;
            set => SetProperty(ref _exception, value);
        }
    }
}
