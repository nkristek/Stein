using System;
using System.Windows.Input;
using WpfBase.ViewModels;

namespace WpfBase.Commands
{
    /// <summary>
    /// ICommand implementation with INotifyPropertyChanged support
    /// </summary>
    public abstract class BindableCommand
        : ComputedBindableBase, ICommand
    {
        private bool _IsWorking;
        /// <summary>
        /// Indicates if the command is working at the moment
        /// </summary>
        public bool IsWorking
        {
            get
            {
                return _IsWorking;
            }

            set
            {
                if (SetProperty(ref _IsWorking, value))
                    RaiseCanExecuteChanged();
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

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
