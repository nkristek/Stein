using System.Windows;
using System.Windows.Shell;

namespace Stein.Views
{
    public static class TaskbarService
    {
        /// <summary>
        /// Sets a TaskbarItemInfo object on the window if it not already exists and sets the state
        /// </summary>
        /// <param name="window">Window on which the TaskbarItemInfo object should be set</param>
        /// <param name="progressState">Progress state which should be set</param>
        public static void SetTaskbarProgressState(Window window, TaskbarItemProgressState progressState)
        {
            if (window.TaskbarItemInfo == null)
                window.TaskbarItemInfo = new TaskbarItemInfo();

            if (window.TaskbarItemInfo.ProgressState != progressState)
                window.TaskbarItemInfo.ProgressState = progressState;
        }

        /// <summary>
        /// Sets the progress on a TaskbarItemInfo object
        /// </summary>
        /// <param name="window">Window on which the progress should be set</param>
        /// <param name="progress">Progress to set</param>
        public static void SetTaskbarProgress(Window window, double progress)
        {
            SetTaskbarProgressState(window, TaskbarItemProgressState.Normal);
            window.TaskbarItemInfo.ProgressValue = progress;
        }

        /// <summary>
        /// Removes the TaskbarItemInfo object from the Window
        /// </summary>
        /// <param name="window"></param>
        public static void UnsetTaskBarProgressState(Window window)
        {
            window.TaskbarItemInfo = null;
        }
    }
}
