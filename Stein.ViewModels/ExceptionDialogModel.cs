﻿using NKristek.Smaragd.ViewModels;
using Stein.Localizations;

namespace Stein.ViewModels
{
    public sealed class ExceptionDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        public override string Title => $"{Strings.Error}: {Exception?.TypeName}";

        private ExceptionViewModel _exception;

        /// <summary>
        /// Contains information about an <see cref="Exception"/> that was thrown.
        /// </summary>
        public ExceptionViewModel Exception
        {
            get => _exception;
            set => SetProperty(ref _exception, value, out _);
        }
    }
}