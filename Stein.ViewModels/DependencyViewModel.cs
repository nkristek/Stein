using System;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class DependencyViewModel
        : ViewModel
    {
        private string _name;

        /// <summary>
        /// Name of the application
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private Uri _uri;

        /// <summary>
        /// Uri of the application
        /// </summary>
        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value, out _);
        }
        
        public ViewModelCommand<DependencyViewModel> OpenUriCommand { get; set; }
    }
}
