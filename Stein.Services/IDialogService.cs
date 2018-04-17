using nkristek.MVVMBase.ViewModels;
using System;

namespace Stein.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows the corresponding popup of the <see cref="ViewModel"/>
        /// </summary>
        /// <param name="contextViewModel"></param>
        void ShowPopup(ViewModel contextViewModel);

        /// <summary>
        /// Shows the corresponding dialog of the <see cref="DialogModel"/>
        /// </summary>
        /// <param name="dialogViewModel">The <see cref="DialogModel"/> to show</param>
        /// <returns>The result of the dialog</returns>
        bool? ShowDialog(DialogModel dialogViewModel);

        /// <summary>
        /// Shows a dialog with the given message
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);

        /// <summary>
        /// Shows a dialog with the given <see cref="Exception"/>
        /// </summary>
        /// <param name="exception"></param>
        void ShowError(Exception exception);
    }
}
