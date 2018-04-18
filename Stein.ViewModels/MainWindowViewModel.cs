using System.Collections.ObjectModel;
using nkristek.MVVMBase.Commands;
using nkristek.MVVMBase.ViewModels;
using Stein.Services;
using Stein.Services.Types;
using Stein.ViewModels.Commands.MainWindowViewModelCommands;

namespace Stein.ViewModels
{
    public class MainWindowViewModel
        : ViewModel
    {
        public MainWindowViewModel()
        {
            RefreshApplicationsCommand = new RefreshApplicationsCommand(this);
            AddApplicationCommand = new AddApplicationCommand(this);
            CancelOperationCommand = new CancelOperationCommand(this);
            ShowInfoDialogCommand = new ShowInfoDialogCommand(this);

            RefreshApplicationsCommand.Execute(null);
        }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public ViewModelCommand<MainWindowViewModel> CancelOperationCommand { get; }

        public ViewModelCommand<MainWindowViewModel> ShowInfoDialogCommand { get; }
        /// <summary>
        /// List of all applications
        /// </summary>
        public ObservableCollection<ApplicationViewModel> Applications { get; } = new ObservableCollection<ApplicationViewModel>();
        
        private InstallationViewModel _currentInstallation;
        /// <summary>
        /// Is set if an operation is in progress
        /// </summary>
        public InstallationViewModel CurrentInstallation
        {
            get => _currentInstallation;
            set
            {
                if (SetProperty(ref _currentInstallation, value))
                    ProgressBarService.Instance.SetState(value != null ? ProgressBarState.Indeterminate : ProgressBarState.None);
            }
        }

        private InstallationResultViewModel _installationResult;
        /// <summary>
        /// Is set if an operation was finished
        /// </summary>
        public InstallationResultViewModel InstallationResult
        {
            get => _installationResult;
            set => SetProperty(ref _installationResult, value);
        }
    }
}
