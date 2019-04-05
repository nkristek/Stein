using NKristek.Smaragd.ViewModels;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class ExceptionDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        public override string Title => $"{Strings.Error}: {Exception?.TypeName}";

        private ExceptionViewModel _exception;
        
        public ExceptionViewModel Exception
        {
            get => _exception;
            set => SetProperty(ref _exception, value, out _);
        }
    }
}
