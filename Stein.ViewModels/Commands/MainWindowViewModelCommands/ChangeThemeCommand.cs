using System;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class ChangeThemeCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        public ChangeThemeCommand(MainWindowViewModel parent, IDialogService dialogService) 
            : base(parent)
        {
            _dialogService = dialogService;
        }

        protected override void Execute(MainWindowViewModel viewModel, object parameter)
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
                    default: break;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }
}
