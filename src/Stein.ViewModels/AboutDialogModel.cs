using System;
using System.Collections.ObjectModel;
using System.Reflection;
using NKristek.Smaragd.ViewModels;
using Stein.Localization;

namespace Stein.ViewModels
{
    public sealed class AboutDialogModel
        : DialogModel
    {
        public AboutDialogModel()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly.GetName();
            var description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            var publisher = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            Title = Strings.About;
            Name = assemblyName.Name;
            Description = description?.Description;
            Version = assemblyName.Version;
            Copyright = copyright?.Copyright;
            AdditionalNotes = "";
            Uri = new Uri("https://github.com/nkristek/Stein");
            Publisher = publisher?.Company;
        }
        
        public ObservableCollection<DependencyViewModel> Dependencies { get; } = new ObservableCollection<DependencyViewModel>();

        private string _name;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, out _);
        }

        private string _description;
        
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, out _);
        }

        private Version _version;
        
        public Version Version
        {
            get => _version;
            set => SetProperty(ref _version, value, out _);
        }

        private string _copyright;
        
        public string Copyright
        {
            get => _copyright;
            set => SetProperty(ref _copyright, value, out _);
        }

        private string _additionalNotes;
        
        public string AdditionalNotes
        {
            get => _additionalNotes;
            set => SetProperty(ref _additionalNotes, value, out _);
        }

        private Uri _uri;
        
        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value, out _);
        }

        private string _publisher;
        
        public string Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value, out _);
        }
    }
}
