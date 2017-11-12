using System;
using System.Runtime.CompilerServices;

namespace WpfBase.ViewModels
{
    public abstract class ViewModel
        : ComputedBindableBase
    {
        public ViewModel(ViewModel parent = null, object view = null)
        {
            Parent = parent;
            View = view;
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
                    if (Parent != null && value)
                        Parent.IsDirty = true;
                    OnIsDirtyChanged(value);
                }
            }
        }

        protected virtual void OnIsDirtyChanged(bool newValue) { }

        private WeakReference<ViewModel> _Parent;

        public ViewModel Parent
        {
            get
            {
                if (_Parent != null && _Parent.TryGetTarget(out ViewModel parent))
                    return parent;
                return null;
            }

            set
            {
                if (Parent == value) return;
                _Parent = value != null ? new WeakReference<ViewModel>(value) : null;
                OnPropertyChanged();
            }
        }

        public T FirstParentOfType<T>() where T : ViewModel
        {
            if (_Parent != null && _Parent.TryGetTarget(out ViewModel parent))
            {
                return parent as T ?? parent.FirstParentOfType<T>();
            }
            return null;
        }

        private WeakReference<object> _View;

        public object View
        {
            get
            {
                if (_View != null && _View.TryGetTarget(out object view))
                    return view;
                return Parent?.View;
            }

            set
            {
                if (View == value) return;
                _View = value != null ? new WeakReference<object>(value) : null;
                OnPropertyChanged();
            }
        }

        public ViewModel TopMost
        {
            get
            {
                return Parent?.TopMost ?? this;
            }
        }

        protected sealed override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            IsDirty = true;
            base.OnPropertyChanged(propertyName);
        }
    }
}
