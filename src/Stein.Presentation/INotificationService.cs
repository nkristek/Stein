using System;

namespace Stein.Presentation
{
    public interface INotificationService
        : IDisposable
    {
        void ShowInfo(string message, Action? onClick = null);

        void ShowSuccess(string message, Action? onClick = null);

        void ShowWarning(string message, Action? onClick = null);

        void ShowError(string message, Action? onClick = null);
    }
}
