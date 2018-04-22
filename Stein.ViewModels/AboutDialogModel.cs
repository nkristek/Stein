using System;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using Stein.ViewModels.Commands.AboutDialogModelCommands;

namespace Stein.ViewModels
{
    public class AboutDialogModel
        : DialogModel
    {
        public AboutDialogModel()
        {
            OpenUriCommand = new OpenUriCommand(this);
        }

        public ViewModelCommand<AboutDialogModel> OpenUriCommand { get; }

        private string _name;

        /// <summary>
        /// Name of the application
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        /// <summary>
        /// Description of the application
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Version _version;
        /// <summary>
        /// Version of the application
        /// </summary>
        public Version Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private string _copyright;
        /// <summary>
        /// Copyright of the application
        /// </summary>
        public string Copyright
        {
            get => _copyright;
            set => SetProperty(ref _copyright, value);
        }

        private string _additionalNotes;
        /// <summary>
        /// Additional notes of the application
        /// </summary>
        public string AdditionalNotes
        {
            get => _additionalNotes;
            set => SetProperty(ref _additionalNotes, value);
        }

        private Uri _uri;
        /// <summary>
        /// Uri of the application
        /// </summary>
        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        private string _publisher;
        /// <summary>
        /// Publisher of the application
        /// </summary>
        public string Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        //public ImageSource _ApplicationLogo;
        //private ImageSource _PublisherLogo;
    }
}
