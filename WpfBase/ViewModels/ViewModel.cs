using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBase.ViewModels
{
    public abstract class ViewModel
        : ComputedBindableBase
    {
        public ViewModel(ViewModel parentViewModel, object parentView)
        {
            ParentViewModel = parentViewModel;
            ParentView = parentView;
        }

        private bool _IsDirty;

        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }

            set
            {
                if (SetProperty(ref _IsDirty, value))
                {
                    if (ParentViewModel != null && value)
                        ParentViewModel.IsDirty = true;
                }
            }
        }

        private WeakReference<ViewModel> _parentViewModel;

        public ViewModel ParentViewModel
        {
            get
            {
                if (_parentViewModel != null && _parentViewModel.TryGetTarget(out ViewModel parentViewModel))
                    return parentViewModel;
                return null;
            }

            private set
            {
                _parentViewModel = value != null ? new WeakReference<ViewModel>(value) : null;
            }
        }

        private WeakReference<object> _parentView;

        public object ParentView
        {
            get
            {
                if (_parentView != null && _parentView.TryGetTarget(out object parentView))
                    return parentView;
                return null;
            }

            private set
            {
                _parentView = value != null ? new WeakReference<object>(value) : null;
            }
        }

        public ViewModel TopMostViewModel
        {
            get
            {
                return ParentViewModel?.TopMostViewModel ?? this;
            }
        }

        protected sealed override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            IsDirty = true;
            base.OnPropertyChanged(propertyName);
        }
    }
}
