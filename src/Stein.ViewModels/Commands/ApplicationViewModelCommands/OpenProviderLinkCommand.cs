using System;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class OpenProviderLinkCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        private readonly IUriService _uriService;

        public OpenProviderLinkCommand(IUriService uriService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <inheritdoc />
        [CanExecuteSource(nameof(ApplicationViewModel.ProviderLink), nameof(ApplicationViewModel.IsUpdating))]
        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            return !String.IsNullOrEmpty(viewModel.ProviderLink) && !viewModel.IsUpdating;
        }

        /// <inheritdoc />
        protected override void Execute(ApplicationViewModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.ProviderLink);
        }
    }
}
