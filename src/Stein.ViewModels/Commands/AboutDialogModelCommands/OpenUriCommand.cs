using System;
using NKristek.Smaragd.Attributes;
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
        [CanExecuteSource(nameof(AboutDialogModel.Uri))]
        protected override bool CanExecute(AboutDialogModel viewModel, object parameter)
        {
            return viewModel.Uri != null && !String.IsNullOrEmpty(viewModel.Uri.AbsoluteUri);
        }

        /// <inheritdoc />
        protected override void Execute(AboutDialogModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.Uri);
        }
    }
}
