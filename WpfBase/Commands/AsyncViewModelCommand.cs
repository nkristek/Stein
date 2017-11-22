using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WpfBase.ViewModels;

namespace WpfBase.Commands
{
    public abstract class AsyncViewModelCommand<TViewModel>
        : AsyncCommand, INotifyPropertyChanged where TViewModel : ViewModel
    {
        public AsyncViewModelCommand(TViewModel parent)
        {
            Parent = parent;
        }

        public virtual bool CanExecute(TViewModel viewModel, object view, object parameter)
        {
            return true;
        }

        public abstract Task ExecuteAsync(TViewModel viewModel, object view, object parameter);

        private bool _IsWorking;
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

        private WeakReference<TViewModel> _Parent;
        private TViewModel Parent
        {
            get
            {
                if (_Parent != null && _Parent.TryGetTarget(out TViewModel parent))
                    return parent;
                return null;
            }

            set
            {
                if (Parent == value) return;
                _Parent = value != null ? new WeakReference<TViewModel>(value) : null;
            }
        }

        public Type AcceptedViewModelType
        {
            get
            {
                return typeof(TViewModel);
            }
        }

        public sealed override bool CanExecute(object parameter)
        {
            return Parent != null && !IsWorking && CanExecute(Parent, Parent?.View, parameter);
        }

        public sealed override async Task ExecuteAsync(object parameter)
        {
            if (!CanExecute(Parent, Parent?.View, parameter))
                throw new InvalidOperationException("canexecute");

            if (IsWorking)
                throw new InvalidOperationException("isworking");

            try
            {
                IsWorking = true;
                await ExecuteAsync(Parent, Parent?.View, parameter);
            }
            catch (Exception exception)
            {
                try
                {
                    OnThrownExeption(Parent, Parent?.View, parameter, exception);
                }
                catch { }
            }
            finally
            {
                IsWorking = false;
            }
        }

        /// <summary>
        /// Will be called when ExecuteAsync throws an exception
        /// </summary>
        /// <returns></returns>
        public virtual void OnThrownExeption(TViewModel viewModel, object view, object parameter, Exception exception) { }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }
        
        protected virtual void OnPropertyChanged(string propertyName) { }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
