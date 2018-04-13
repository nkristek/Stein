using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Shell;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using nkristek.Stein.Services;
using nkristek.Stein.Commands.MainWindowViewModelCommands;

namespace nkristek.Stein.ViewModels
{
    public class MainWindowViewModel
        : ViewModel
    {
        public MainWindowViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            RefreshApplicationsCommand = new RefreshApplicationsCommand(this);
            AddApplicationCommand = new AddApplicationCommand(this);
            CancelOperationCommand = new CancelOperationCommand(this);
            ShowInfoDialogCommand = new ShowInfoDialogCommand(this);
        }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public ViewModelCommand<MainWindowViewModel> CancelOperationCommand { get; private set; }

        public AsyncViewModelCommand<MainWindowViewModel> ShowInfoDialogCommand { get; private set; }
        /// <summary>
        /// List of all applications
        /// </summary>
        public ObservableCollection<ApplicationViewModel> Applications { get; } = new ObservableCollection<ApplicationViewModel>();
        
        private InstallationViewModel _CurrentInstallation;
        /// <summary>
        /// Is set if an operation is in progress
        /// </summary>
        public InstallationViewModel CurrentInstallation
        {
            get { return _CurrentInstallation; }
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

        private InstallationResultViewModel _InstallationResult;
        /// <summary>
        /// Is set if an operation was finished
        /// </summary>
        public InstallationResultViewModel InstallationResult
        {
            get { return _InstallationResult; }
            set { SetProperty(ref _InstallationResult, value); }
        }
    }
}
