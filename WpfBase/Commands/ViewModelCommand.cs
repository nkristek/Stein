﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfBase.ViewModels;

namespace WpfBase.Commands
{
    public abstract class ViewModelCommand<TViewModel>
        : Command, INotifyPropertyChanged where TViewModel : ViewModel
    {
        public ViewModelCommand(TViewModel parent)
        {
            Parent = parent;
        }
        
        public virtual bool CanExecute(TViewModel viewModel, object view, object parameter)
        {
            return true;
        }

        public abstract void Execute(TViewModel viewModel, object view, object parameter);

        private bool _IsWorking;

        public bool IsWorking
        {
            get
            {
                return _IsWorking;
            }

            set
            {
                SetProperty(ref _IsWorking, value);
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
            return !IsWorking && CanExecute(Parent, Parent?.View, parameter);
        }

        public sealed override void Execute(object parameter)
        {
            if (!CanExecute(Parent, Parent?.View, parameter))
                throw new InvalidOperationException("canexecute");

            if (IsWorking)
                throw new InvalidOperationException("isworking");

            IsWorking = true;
            RaiseCanExecuteChanged();

            Execute(Parent, Parent?.View, parameter);

            IsWorking = false;
            RaiseCanExecuteChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
