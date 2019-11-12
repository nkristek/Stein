using System;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class DependencyViewModel
        : ViewModel
    {
        private string? _name;
        
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Uri? _uri;
        
        public Uri? Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        private IViewModelCommand<DependencyViewModel>? _openUriCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<DependencyViewModel>? OpenUriCommand
        {
            get => _openUriCommand;
            set
            {
                if (SetProperty(ref _openUriCommand, value, out var oldValue))
                {
                    if (oldValue != null)
                        oldValue.Context = null;
                    if (value != null)
                        value.Context = this;
                }
            }
        }
    }
}
