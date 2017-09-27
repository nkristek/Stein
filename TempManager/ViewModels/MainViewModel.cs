using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempManager.Commands;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace TempManager.ViewModels
{
    class MainViewModel
        : ViewModel
    {
        public MainViewModel(ViewModel parentViewModel, object parentView) : base(parentViewModel, parentView) { }

        public ViewModelCommand<MainViewModel> UninstallCommand
        {
            get
            {
                return new UninstallCommand(this, ParentView);
            }
        }
    }
}
