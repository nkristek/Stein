using System;
using WpfBase.ViewModels;

namespace WpfBase.Commands
{
    /// <summary>
    ///BindableCommand implementation with ViewModel parameters in command methods
    /// </summary>
    /// <typeparam name="TViewModel">Subclass of ViewModel</typeparam>
    public abstract class ViewModelCommand<TViewModel>
        : BindableCommand where TViewModel : ViewModel
    {
        public ViewModelCommand(TViewModel parent)
        {
            Parent = parent;
        }

        private WeakReference<TViewModel> _Parent;
        /// <summary>
        /// Parent of this ViewModelCommand which is used when calling CanExecute/Execute or OnThrownException
        /// </summary>
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

        public virtual bool CanExecute(TViewModel viewModel, object view, object parameter)
        {
            return true;
        }

        public abstract void Execute(TViewModel viewModel, object view, object parameter);

        /// <summary>
        /// Will be called when ExecuteAsync throws an exception
        /// </summary>
        /// <returns></returns>
        public virtual void OnThrownExeption(TViewModel viewModel, object view, object parameter, Exception exception) { }

        public override sealed bool CanExecute(object parameter)
        {
            return Parent != null && !IsWorking && CanExecute(Parent, Parent?.View, parameter);
        }

        public override sealed void Execute(object parameter)
        {
            if (!CanExecute(Parent, Parent?.View, parameter))
                throw new InvalidOperationException("canexecute");

            if (IsWorking)
                throw new InvalidOperationException("isworking");

            try
            {
                IsWorking = true;
                Execute(Parent, Parent?.View, parameter);
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
    }
}
