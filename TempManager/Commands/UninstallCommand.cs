using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempManager.ViewModels;
using WpfBase.Commands;

namespace TempManager.Commands
{
    class UninstallCommand
        : ViewModelCommand<MainViewModel>
    {
        public UninstallCommand(MainViewModel parentViewModel, object parentView) : base(parentViewModel, parentView) { }

        public override bool CanExecute(MainViewModel viewModel, object view, object parameter)
        {
            return base.CanExecute(viewModel, view, parameter);
        }
        
        public override void Execute(MainViewModel viewModel, object view, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
