﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfBase.Commands
{
    /// <summary>
    /// IAsyncCommand implementation
    /// </summary>
    public abstract class AsyncCommand
        : IAsyncCommand
    {
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract Task ExecuteAsync(object parameter);

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        private EventHandler _internalCanExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _internalCanExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                _internalCanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            _internalCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
