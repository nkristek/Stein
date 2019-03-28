using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using NKristek.Smaragd.ViewModels;
using Stein.Localizations;
using Stein.Presentation;

namespace Stein.Views.Services
{
    public class WpfDialogService 
        : IDialogService
    {
        private readonly Stack<Window> _windowStack = new Stack<Window>();

        public WpfDialogService(Window root)
        {
            _windowStack.Push(root);
        }

        /// <inheritdoc />
        public bool? ShowDialog(IDialogModel dialogModel)
        {
            var dialog = CreateView<Window>(dialogModel);
            dialog.Owner = _windowStack.Peek();

            _windowStack.Push(dialog);
            
            bool? result;
            try
            {
                result = dialog.ShowDialog();
            }
            finally
            {
                _windowStack.Pop();
            }

            return result;
        }
        
        private TView CreateView<TView>(IViewModel contextViewModel) where TView : FrameworkElement
        {
            if (contextViewModel == null)
                throw new ArgumentNullException(nameof(contextViewModel));

            var resourceKey = contextViewModel.GetType();
            var view = Application.Current.TryFindResource(resourceKey) ?? Application.Current.MainWindow?.TryFindResource(resourceKey);
            if (view == null)
                throw new NotSupportedException(Strings.NoDialogExistsForDialogModel);

            if (!(view is TView))
                throw new Exception(String.Format(Strings.ViewHasWrongTypeX, resourceKey.Name, view.GetType().Name, typeof(TView).Name));

            var viewAsTargetType = (TView)view;
            viewAsTargetType.DataContext = contextViewModel;

            return viewAsTargetType;
        }

        /// <inheritdoc />
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <inheritdoc />
        public void ShowError(Exception exception)
        {
            // TODO: create ErrorDialogModel
            var exceptionMessage = BuildExceptionMessage(exception);
            ShowMessage(exceptionMessage);
        }
        
        private static string BuildExceptionMessage(Exception exception)
        {
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(exception.Message);

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                messageBuilder.AppendLine(innerException.Message);
                innerException = innerException.InnerException;
            }

            messageBuilder.AppendLine(exception.StackTrace);

            return messageBuilder.ToString();
        }
    }
}
