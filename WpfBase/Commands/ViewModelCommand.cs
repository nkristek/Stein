using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBase.ViewModels;

namespace WpfBase.Commands
{
    public abstract class ViewModelCommand<TViewModel>
        : Command where TViewModel : ViewModel
    {
        public ViewModelCommand(TViewModel parentViewModel, object parentView)
        {
            _ParentViewModel = parentViewModel;
            _ParentView = parentView;
        }

        private WeakReference<TViewModel> _parentViewModel;

        private TViewModel _ParentViewModel
        {
            get
            {
                if (_parentViewModel != null && _parentViewModel.TryGetTarget(out TViewModel parentViewModel))
                    return parentViewModel;
                return null;
            }

            set
            {
                _parentViewModel = value != null ? new WeakReference<TViewModel>(value) : null;
            }
        }

        private WeakReference<object> _parentView;

        private object _ParentView
        {
            get
            {
                if (_parentView != null && _parentView.TryGetTarget(out object parentView))
                    return parentView;
                return null;
            }

            set
            {
                _parentView = value != null ? new WeakReference<object>(value) : null;
            }
        }

        public virtual bool CanExecute(TViewModel viewModel, object view, object parameter)
        {
            return true;
        }

        public abstract void Execute(TViewModel viewModel, object view, object parameter);

        public Type AcceptedViewModelType
        {
            get
            {
                return typeof(TViewModel);
            }
        }

        public sealed override bool CanExecute(object parameter)
        {
            return CanExecute(_ParentViewModel, _ParentView, parameter);
        }

        public sealed override void Execute(object parameter)
        {
            Execute(_ParentViewModel, _ParentView, parameter);
        }
    }
}
