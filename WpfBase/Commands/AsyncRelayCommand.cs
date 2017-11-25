﻿using System;
using System.Threading.Tasks;

namespace WpfBase.Commands
{
    /// <summary>
    /// Asynchronous implementation of a RelayCommand
    /// </summary>
    public class AsyncRelayCommand
        : AsyncCommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public AsyncRelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(() =>
            {
                _execute?.Invoke(parameter);
            });
        }
    }
}
