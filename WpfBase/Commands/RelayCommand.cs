using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBase.Commands
{
    public class RelayCommand
        : Command
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
