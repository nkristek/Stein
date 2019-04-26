using NKristek.Smaragd.ViewModels;

namespace Stein.Presentation
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows the corresponding dialog of the <see cref="DialogModel"/>. It will not wait for the opened dialog to close.
        /// </summary>
        /// <param name="dialogModel">The <see cref="DialogModel"/> to show.</param>
        void Show(IDialogModel dialogModel);

        /// <summary>
        /// Shows the corresponding dialog of the <see cref="DialogModel"/> and waits for the result.
        /// </summary>
        /// <param name="dialogModel">The <see cref="DialogModel"/> to show.</param>
        /// <returns>The result of the dialog.</returns>
        bool? ShowDialog(IDialogModel dialogModel);

        /// <summary>
        /// Shows a dialog with the given <paramref name="message"/>.
        /// It contains an Ok button.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="title">Title of the dialog.</param>
        bool? ShowInfoDialog(string message, string title = null);

        /// <summary>
        /// Shows a confirm dialog with the given <paramref name="message"/>.
        /// It contains an Yes and No button.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="title">Title of the dialog.</param>
        bool? ShowConfirmDialog(string message, string title = null);

        /// <summary>
        /// Shows a warning dialog with the given <paramref name="message"/>.
        /// It contains an Ok and Cancel button.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="title">Title of the dialog.</param>
        bool? ShowWarningDialog(string message, string title = null);

        /// <summary>
        /// Shows an error dialog with the given <paramref name="message"/>.
        /// It contains an Ok button.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="title">Title of the dialog.</param>
        bool? ShowErrorDialog(string message, string title = null);

        /// <summary>
        /// Shows a dialog to select a file.
        /// </summary>
        /// <param name="filePath">Selected file path if the result is <c>true</c>, else <c>null</c>.</param>
        /// <param name="title">Optional title of the dialog.</param>
        /// <returns><c>true</c> if the user successfully selected a file, <c>false</c> if cancelled and <c>null</c> if closed.</returns>
        bool? ShowSelectFileDialog(out string filePath, string title = null);

        /// <summary>
        /// Shows a dialog to select a folder.
        /// </summary>
        /// <param name="folderPath">Selected folder path if the result is <c>true</c>, else <c>null</c>.</param>
        /// <param name="title">Optional title of the dialog.</param>
        /// <returns><c>true</c> if the user successfully selected a folder, <c>false</c> if cancelled and <c>null</c> if closed.</returns>
        bool? ShowSelectFolderDialog(out string folderPath, string title = null);
    }
}
