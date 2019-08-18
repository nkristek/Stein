using System;
using System.ComponentModel;
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
        protected override void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(UpdateDialogModel.UpdateUri)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
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
