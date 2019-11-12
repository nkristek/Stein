using System;
using System.Collections.ObjectModel;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class AboutDialogModel
        : DialogModel
    {
        public ObservableCollection<DependencyViewModel> Dependencies { get; } = new ObservableCollection<DependencyViewModel>();

        private string? _name;
        
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _description;
        
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Version? _version;
        
        public Version? Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private string? _copyright;
        
        public string? Copyright
        {
            get => _copyright;
            set => SetProperty(ref _copyright, value);
        }

        private string? _additionalNotes;
        
        public string? AdditionalNotes
        {
            get => _additionalNotes;
            set => SetProperty(ref _additionalNotes, value);
        }

        private Uri? _uri;
        
        public Uri? Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        private string? _publisher;
        
        public string? Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        private IViewModelCommand<AboutDialogModel>? _openUriCommand;

        [IsDirtyIgnored]
        [IsReadOnlyIgnored]
        public IViewModelCommand<AboutDialogModel>? OpenUriCommand
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
