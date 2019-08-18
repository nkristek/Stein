using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class ExceptionViewModel
        : ViewModel
    {
        private string _localizedReason = Strings.ErrorOccured;
        
        public string LocalizedReason
        {
            get => _localizedReason;
            set => SetProperty(ref _localizedReason, value);
        }

        private string _typeName;
        
        public string TypeName
        {
            get => _typeName;
            set => SetProperty(ref _typeName, value);
        }

        private string _message;
        
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _stackTrace;
        
        public string StackTrace
        {
            get => _stackTrace;
            set => SetProperty(ref _stackTrace, value);
        }

        [PropertySource(nameof(TypeName), nameof(Message), nameof(StackTrace), nameof(InnerExceptions))]
        public string ExceptionText => GenerateExceptionText(this);
        
        private static string GenerateExceptionText(ExceptionViewModel viewModel, int indent = 0)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ', indent);
            stringBuilder.AppendLine($"{viewModel.TypeName}: {viewModel.Message}");
            stringBuilder.Append(' ', indent);
            stringBuilder.AppendLine(viewModel.StackTrace);
            if (viewModel.InnerExceptions.Any())
            {
                stringBuilder.Append(' ', indent);
                stringBuilder.AppendLine($"Inner exception{(viewModel.InnerExceptions.Count > 1 ? "s" : String.Empty)}:");
                foreach (var innerException in viewModel.InnerExceptions)
                {
                    stringBuilder.Append(' ', indent);
                    stringBuilder.AppendLine(GenerateExceptionText(innerException, indent + 4));
                }
            }

            return stringBuilder.ToString();
        }
        
        public ObservableCollection<ExceptionViewModel> InnerExceptions { get; } = new ObservableCollection<ExceptionViewModel>();

        private IViewModelCommand<ExceptionViewModel> _copyExceptionDetailsToClipboardCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<ExceptionViewModel> CopyExceptionDetailsToClipboardCommand
        {
            get => _copyExceptionDetailsToClipboardCommand;
            set
            {
                if (SetProperty(ref _copyExceptionDetailsToClipboardCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }
    }
}
