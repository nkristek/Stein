using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TempManager.Commands.MainViewModelCommands;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace TempManager.ViewModels
{
    public class MainViewModel
        : ViewModel
    {
        public MainViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }
        
        public ViewModelCommand<MainViewModel> AddApplicationCommand
        {
            get
            {
                return new AddApplicationCommand(this);
            }
        }

        public ViewModelCommand<MainViewModel> DeleteApplicationCommand
        {
            get
            {
                return new DeleteApplicationCommand(this);
            }
        }

        public ViewModelCommand<MainViewModel> RefreshApplicationsCommand
        {
            get
            {
                return new RefreshApplicationsCommand(this);
            }
        }

        private ObservableCollection<ApplicationViewModel> _Applications = new ObservableCollection<ApplicationViewModel>();

        public ObservableCollection<ApplicationViewModel> Applications
        {
            get
            {
                return _Applications;
            }

            set
            {
                SetProperty(ref _Applications, value);
            }
        }

        private InstallationViewModel _CurrentInstallation;

        public InstallationViewModel CurrentInstallation
        {
            get
            {
                return _CurrentInstallation;
            }

            set
            {
                if (SetProperty(ref _CurrentInstallation, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
