using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.WindowsAPICodePack.Dialogs;
using NKristek.Smaragd.ViewModels;
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
            var dialog = CreateDialogWindow(dialogModel);
            _windowStack.Push(dialog);
            dialog.Show();
        }

        /// <inheritdoc />
        public bool? ShowDialog(IDialogModel dialogModel)
        {
            var dialog = CreateDialogWindow(dialogModel);
            if (_windowStack.Any())
            {
                dialog.Owner = _windowStack.Peek();
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
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

        private Window CreateDialogWindow(IDialogModel dialogModel)
        {
            var window = new DialogWindow
            {
                DataContext = dialogModel
            };
            window.SetBinding(Window.TitleProperty, new Binding(nameof(dialogModel.Title))
            {
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            return window;
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

        /// <inheritdoc />
        public bool? ShowSelectFileDialog([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string filePath, string title = null)
        {
            filePath = null;
            using var dialog = new CommonOpenFileDialog();
            if (title != null)
                dialog.Title = title;
            dialog.Multiselect = false;

            switch (dialog.ShowDialog())
            {
                case CommonFileDialogResult.Ok:
                    filePath = dialog.FileName;
                    return true;
                case CommonFileDialogResult.Cancel: return false;
                default: return null;
            }
        }

        private static bool? GetMessageBoxResult(MessageBoxResult messageBoxResult)
        {
            switch (messageBoxResult)
            {
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes: return true;
                case MessageBoxResult.No: return false;
                default: return null;
            }
        }

        /// <inheritdoc />
        public bool? ShowSelectFolderDialog([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string folderPath, string title = null)
        {
            folderPath = null;
            using var dialog = new CommonOpenFileDialog();
            if (title != null)
                dialog.Title = title;
            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;

            switch (dialog.ShowDialog())
            {
                case CommonFileDialogResult.Ok:
                    folderPath = dialog.FileName;
                    return true;
                case CommonFileDialogResult.Cancel: return false;
                default: return null;
            }
        }
    }
}
