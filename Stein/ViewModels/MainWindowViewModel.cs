using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Stein.Commands.MainWindowViewModelCommands;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace Stein.ViewModels
{
    public class MainWindowViewModel
        : ViewModel
    {
        public MainWindowViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            RefreshApplicationsCommand = new RefreshApplicationsCommand(this);
            AddApplicationCommand = new AddApplicationCommand(this);
            DeleteApplicationCommand = new DeleteApplicationCommand(this);
        }

        public ViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; private set; }

        public ViewModelCommand<MainWindowViewModel> DeleteApplicationCommand { get; private set; }
        
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; private set; }
        
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
                {
                    CommandManager.InvalidateRequerySuggested();

                    if (View is Window window)
                    {
                        if (value != null)
                        {
                            window.TaskbarItemInfo = new System.Windows.Shell.TaskbarItemInfo()
                            {
                                ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate,
                            };
                        }
                        else
                        {
                            window.TaskbarItemInfo = null;
                        }
                    }
                }
            }
        }
    }
}
