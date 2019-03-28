using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class ExceptionViewModel
        : ViewModel
    {
        private string _typeName;

        /// <summary>
        /// The name of the type of the <see cref="Exception"/>.
        /// </summary>
        public string TypeName
        {
            get => _typeName;
            set => SetProperty(ref _typeName, value, out _);
        }

        private string _message;

        /// <summary>
        /// Message of the <see cref="Exception"/>.
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value, out _);
        }

        private string _stackTrace;

        /// <summary>
        /// Stack trace of the <see cref="Exception"/>.
        /// </summary>
        public string StackTrace
        {
            get => _stackTrace;
            set => SetProperty(ref _stackTrace, value, out _);
        }

        /// <summary>
        /// Inner exceptions of the <see cref="Exception"/>.
        /// </summary>
        public ObservableCollection<ExceptionViewModel> InnerExceptions { get; } = new ObservableCollection<ExceptionViewModel>();
    }
}
