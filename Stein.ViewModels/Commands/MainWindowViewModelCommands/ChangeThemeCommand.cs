using System;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class ChangeThemeCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public ChangeThemeCommand(IDialogService dialogService, IViewModelService viewModelService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object parameter)
        {
            try
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
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
