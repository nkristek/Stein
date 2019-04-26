using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using NKristek.Smaragd.ViewModels;
using Stein.Localization;
using Stein.Presentation;

namespace Stein.Views.Services
{
    public class WpfDialogService 
        : IDialogService
    {
        private readonly Stack<Window> _windowStack = new Stack<Window>();

        /// <inheritdoc />
        public void Show(IDialogModel dialogModel)
        {
            var dialog = CreateView<Window>(dialogModel);
            if (!_windowStack.Any())
                _windowStack.Push(dialog);
            dialog.Show();
        }

        /// <inheritdoc />
        public bool? ShowDialog(IDialogModel dialogModel)
        {
            var dialog = CreateView<Window>(dialogModel);
            if (_windowStack.Any())
                dialog.Owner = _windowStack.Peek();
            _windowStack.Push(dialog);

            try
            {
                return dialog.ShowDialog();
            }
            finally
            {
                _windowStack.Pop();
            }
        }
        
        private TView CreateView<TView>(IViewModel contextViewModel) where TView : FrameworkElement
        {
            if (contextViewModel == null)
                throw new ArgumentNullException(nameof(contextViewModel));

            var resourceKey = contextViewModel.GetType();
            var view = Application.Current?.TryFindResource(resourceKey) ?? Application.Current?.MainWindow?.TryFindResource(resourceKey);
            if (view == null)
                throw new NotSupportedException(Strings.NoDialogExistsForDialogModel);
            if (!(view is TView viewAsTargetType))
                throw new Exception(String.Format(Strings.ViewHasWrongTypeX, resourceKey.Name, view.GetType().Name, typeof(TView).Name));
            viewAsTargetType.DataContext = contextViewModel;
            return viewAsTargetType;
        }

        /// <inheritdoc />
        public bool? ShowInfoDialog(string message, string title = null)
        {
            var result = MessageBox.Show(_windowStack.Peek(), message, title ?? String.Empty, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            return GetMessageBoxResult(result);
        }

        /// <inheritdoc />
        public bool? ShowConfirmDialog(string message, string title = null)
        {
            var result = MessageBox.Show(_windowStack.Peek(), message, title ?? String.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            return GetMessageBoxResult(result);
        }

        /// <inheritdoc />
        public bool? ShowWarningDialog(string message, string title = null)
        {
            var result = MessageBox.Show(_windowStack.Peek(), message, title ?? String.Empty, MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.OK);
            return GetMessageBoxResult(result);
        }

        /// <inheritdoc />
        public bool? ShowErrorDialog(string message, string title = null)
        {
            var result = MessageBox.Show(_windowStack.Peek(), message, title ?? String.Empty, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            return GetMessageBoxResult(result);
        }

        private static bool? GetMessageBoxResult(MessageBoxResult messageBoxResult)
        {
            switch (messageBoxResult)
            {
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes: return true;
                case MessageBoxResult.No: return false;
                case MessageBoxResult.None:
                case MessageBoxResult.Cancel: return null;
            }

            return null;
        }

        /// <inheritdoc />
        public bool? ShowSelectFolderDialog(out string folderPath, string title = null)
        {
            folderPath = null;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = Strings.SelectFolder;
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;

                var result = dialog.ShowDialog();
                switch (result)
                {
                    case CommonFileDialogResult.None: return null;
                    case CommonFileDialogResult.Cancel: return false;
                    case CommonFileDialogResult.Ok:
                        folderPath = dialog.FileName;
                        return true;
                    default: return null;
                }
            }
        }
    }
}
