﻿using System;
using System.ComponentModel;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowDialogModelCommands
{
    public class ShowUpdateDialogCommand
        : ViewModelCommand<MainWindowDialogModel>
    {
        private readonly IDialogService _dialogService;

        public ShowUpdateDialogCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <inheritdoc />
        protected override void OnContextPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (e == null
                || String.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(MainWindowDialogModel.AvailableUpdate)))
                NotifyCanExecuteChanged();
        }

        /// <inheritdoc />
        protected override bool CanExecute(MainWindowDialogModel? viewModel, object? parameter)
        {
            return viewModel?.AvailableUpdate != null;
        }

        /// <inheritdoc />
        protected override void Execute(MainWindowDialogModel? viewModel, object? parameter)
        {
            if (viewModel?.AvailableUpdate is UpdateDialogModel update)
                _dialogService.ShowDialog(update);
        }
    }
}
