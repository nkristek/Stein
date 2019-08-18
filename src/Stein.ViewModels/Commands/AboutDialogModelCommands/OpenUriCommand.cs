using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.AboutDialogModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<AboutDialogModel>
    {
        private readonly IUriService _uriService;

        public OpenUriCommand(IUriService uriService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null 
                || String.IsNullOrEmpty(e.PropertyName) 
                || e.PropertyName.Equals(nameof(AboutDialogModel.Uri)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(AboutDialogModel viewModel, object parameter)
        {
            return viewModel.Uri != null;
        }

        /// <inheritdoc />
        protected override void Execute(AboutDialogModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.Uri);
        }
    }
}
