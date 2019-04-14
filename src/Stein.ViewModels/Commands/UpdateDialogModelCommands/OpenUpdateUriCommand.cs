using System;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.UpdateDialogModelCommands
{
    public class OpenUpdateUriCommand
        : ViewModelCommand<UpdateDialogModel>
    {
        private readonly IUriService _uriService;

        public OpenUpdateUriCommand(IUriService uriService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(UpdateDialogModel.UpdateUri))]
        protected override bool CanExecute(UpdateDialogModel viewModel, object parameter)
        {
            return viewModel.UpdateUri != null;
        }

        /// <inheritdoc />
        protected override void Execute(UpdateDialogModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.UpdateUri);
        }
    }
}
