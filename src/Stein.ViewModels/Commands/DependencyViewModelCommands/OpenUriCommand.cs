using System;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.DependencyViewModelCommands
{
    public sealed class OpenUriCommand
        : ViewModelCommand<DependencyViewModel>
    {
        private readonly IUriService _uriService;

        public OpenUriCommand(IUriService uriService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(DependencyViewModel.Uri))]
        protected override bool CanExecute(DependencyViewModel viewModel, object parameter)
        {
            return viewModel.Uri != null && !String.IsNullOrEmpty(viewModel.Uri.AbsoluteUri);
        }

        /// <inheritdoc />
        protected override void Execute(DependencyViewModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.Uri);
        }
    }
}
