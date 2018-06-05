using System;
using System.Windows;
using System.Windows.Shell;
using Stein.Presentation;

namespace Stein.Views.Services
{
    public class WpfTaskbarService
        : IProgressBarService
    {
        private readonly Window _window;

        public WpfTaskbarService(Window rootWindow)
        {
            _window = rootWindow;
        }
        
        public void SetState(ProgressBarState state)
        {
            if (_window.TaskbarItemInfo == null)
                _window.TaskbarItemInfo = new TaskbarItemInfo();

            var taskbarState = GetTaskbarState(state);
            if (_window.TaskbarItemInfo.ProgressState != taskbarState)
                _window.TaskbarItemInfo.ProgressState = taskbarState;
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
            SetState(ProgressBarState.Normal);
            _window.TaskbarItemInfo.ProgressValue = progress;
        }
    }
}
