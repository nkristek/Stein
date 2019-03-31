using System;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class DependencyViewModel
        : ViewModel
    {
        private string _name;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private Uri _uri;
        
        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value, out _);
        }
    }
}
