using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Stein.Services
{
    public static class TaskbarService
    {
        public static void SetTaskbarProgressState(Window window, TaskbarItemProgressState progressState)
        {
            if (window.TaskbarItemInfo == null)
                window.TaskbarItemInfo = new TaskbarItemInfo();

            window.TaskbarItemInfo.ProgressState = progressState;
        }

        public static void SetTaskbarProgress(Window window, double progress)
        {
            SetTaskbarProgressState(window, TaskbarItemProgressState.Normal);
            window.TaskbarItemInfo.ProgressValue = progress;
        }

        public static void UnsetTaskBarProgressState(Window window)
        {
            window.TaskbarItemInfo = null;
        }
    }
}
