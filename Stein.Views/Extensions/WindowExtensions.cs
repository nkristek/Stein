using System;
using System.Threading.Tasks;
using System.Windows;

namespace Stein.Views.Extensions
{
    public static class WindowExtensions
    {
        public static Task<bool?> ShowDialogAsync(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            var completion = new TaskCompletionSource<bool?>();
            window.Dispatcher.BeginInvoke(new Action(() => completion.SetResult(window.ShowDialog())));

            return completion.Task;
        }
    }
}
