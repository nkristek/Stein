using System;
using nkristek.MVVMBase.Commands;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public class ChangeThemeCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public ChangeThemeCommand(MainWindowViewModel parent) : base(parent) { }

        protected override void DoExecute(MainWindowViewModel viewModel, object parameter)
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
    }
}
