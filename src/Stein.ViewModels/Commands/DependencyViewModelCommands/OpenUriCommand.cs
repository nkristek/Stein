using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

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
        protected override void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(DependencyViewModel.Uri)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(DependencyViewModel viewModel, object parameter)
        {
            return viewModel.Uri != null;
        }

        /// <inheritdoc />
        protected override void Execute(DependencyViewModel viewModel, object parameter)
        {
            _uriService.OpenUri(viewModel.Uri);
        }
    }
}
