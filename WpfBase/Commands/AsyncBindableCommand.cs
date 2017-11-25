using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfBase.ViewModels;

namespace WpfBase.Commands
{
    /// <summary>
    /// IAsyncCommand implementation with INotifyPropertyChanged support
    /// </summary>
    public abstract class AsyncBindableCommand
        : ComputedBindableBase, IAsyncCommand
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

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public abstract Task ExecuteAsync(object parameter);
        
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
