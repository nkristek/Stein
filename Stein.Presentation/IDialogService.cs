using System;
using NKristek.Smaragd.ViewModels;

namespace Stein.Presentation
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows the corresponding dialog of the <see cref="DialogModel"/>
        /// </summary>
        /// <param name="dialogModel">The <see cref="DialogModel"/> to show.</param>
        /// <returns>The result of the dialog.</returns>
        bool? ShowDialog(IDialogModel dialogModel);
        
        /// <summary>
        /// Shows a dialog with the given message
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);
    }
}
