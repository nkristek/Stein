using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private int? _CurrentInstallationProgress;

        public int? CurrentInstallationProgress
        {
            get
            {
                return _CurrentInstallationProgress;
            }

            set
            {
                SetProperty(ref _CurrentInstallationProgress, value);
            }
        }

        private string _CurrentInstallationName;

        public string CurrentInstallationName
        {
            get
            {
                return _CurrentInstallationName;
            }

            set
            {
                SetProperty(ref _CurrentInstallationName, value);
            }
        }
    }
}
