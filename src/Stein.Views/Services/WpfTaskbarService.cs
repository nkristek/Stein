using System;
using System.Windows;
using System.Windows.Shell;
using Stein.Presentation;

namespace Stein.Views.Services
{
    public class WpfTaskbarService
        : IProgressBarService
    {
        public void SetState(ProgressBarState state)
        {
            var window = Application.Current?.MainWindow;
            if (window == null)
                return;

            if (window.TaskbarItemInfo == null)
                window.TaskbarItemInfo = new TaskbarItemInfo();

            var taskbarState = GetTaskbarState(state);
            if (window.TaskbarItemInfo.ProgressState != taskbarState)
                window.TaskbarItemInfo.ProgressState = taskbarState;
        }

        private TaskbarItemProgressState GetTaskbarState(ProgressBarState state)
        {
            switch (state)
            {
                case ProgressBarState.None: return TaskbarItemProgressState.None;
                case ProgressBarState.Indeterminate: return TaskbarItemProgressState.Indeterminate;
                case ProgressBarState.Normal: return TaskbarItemProgressState.Normal;
                case ProgressBarState.Error: return TaskbarItemProgressState.Error;
                case ProgressBarState.Paused: return TaskbarItemProgressState.Paused;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        public void SetProgress(double progress)
        {
            var window = Application.Current?.MainWindow;
            if (window == null)
                return;

            SetState(ProgressBarState.Normal);
            window.TaskbarItemInfo.ProgressValue = progress;
        }
    }
}
