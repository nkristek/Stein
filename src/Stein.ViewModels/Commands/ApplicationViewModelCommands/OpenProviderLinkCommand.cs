using System;
using System.ComponentModel;
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
        protected override void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(ApplicationViewModel.ProviderLink))
                || e.PropertyName.Equals(nameof(ApplicationViewModel.IsUpdating)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
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
