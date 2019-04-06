using System;
using System.Threading.Tasks;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowDialogModelCommands
{
    public sealed class ChangeThemeCommand
        : AsyncViewModelCommand<MainWindowDialogModel>
    {
        private readonly IViewModelService _viewModelService;

        public ChangeThemeCommand(IViewModelService viewModelService)
        {
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(MainWindowDialogModel viewModel, object parameter)
        {
            if (parameter is string parameterAsString && Enum.TryParse(parameterAsString, out Theme theme))
            {
                viewModel.CurrentTheme = theme;
                return;
            }

            switch (viewModel.CurrentTheme)
            {
                case Theme.Light:
                    viewModel.CurrentTheme = Theme.Dark;
                    break;
                case Theme.Dark:
                    viewModel.CurrentTheme = Theme.Light;
                    break;
                default:
                    throw new NotSupportedException("Theme not supported.");
            }

            await _viewModelService.SaveViewModelAsync(viewModel);
        }
    }
}
