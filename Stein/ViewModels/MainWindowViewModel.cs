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
using Stein.Services;
using System.Windows.Shell;

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

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> DeleteApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; private set; }
        
        private readonly ObservableCollection<ApplicationViewModel> _Applications = new ObservableCollection<ApplicationViewModel>();
        public ObservableCollection<ApplicationViewModel> Applications
        {
            get
            {
                return _Applications;
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
                    if (View is Window window)
                    {
                        if (value != null)
                            TaskbarService.SetTaskbarProgressState(window, TaskbarItemProgressState.Indeterminate);
                        else
                            TaskbarService.UnsetTaskBarProgressState(window);
                    }
                }
            }
        }
    }
}
