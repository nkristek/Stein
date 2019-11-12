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
        protected override async Task ExecuteAsync(MainWindowDialogModel? viewModel, object? parameter)
        {
            if (viewModel == null)
                return;

            if (parameter is string parameterAsString)
            {
                viewModel.CurrentTheme = (Theme)Enum.Parse(typeof(Theme), parameterAsString);
            }
            else
            {
                viewModel.CurrentTheme = viewModel.CurrentTheme switch
                {
                    Theme.Light => Theme.Dark,
                    Theme.Dark => Theme.Light,
                    _ => Theme.Light,
                };
            }

            await _viewModelService.SaveViewModelAsync(viewModel);
        }
    }
}
