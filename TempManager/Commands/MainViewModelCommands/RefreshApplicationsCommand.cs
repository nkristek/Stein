using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempManager.Services;
using TempManager.ViewModels;
using WpfBase.Commands;

namespace TempManager.Commands.MainViewModelCommands
{
    public class RefreshApplicationsCommand
        : ViewModelCommand<MainViewModel>
    {
        public RefreshApplicationsCommand(MainViewModel parent) : base(parent) { }

        public override bool CanExecute(MainViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override void Execute(MainViewModel viewModel, object view, object parameter)
        {
            viewModel.Applications.Clear();

            foreach (var applicationViewModel in ViewModelService.CreateApplicationViewModels(viewModel))
                viewModel.Applications.Add(applicationViewModel);
        }
    }
}
