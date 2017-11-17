using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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
            EditApplicationCommand = new EditApplicationCommand(this);
            DeleteApplicationCommand = new DeleteApplicationCommand(this);
            CancelOperationCommand = new CancelOperationCommand(this);

            Applications.CollectionChanged += Applications_CollectionChanged;
        }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> AddApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> DeleteApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> RefreshApplicationsCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public AsyncViewModelCommand<MainWindowViewModel> EditApplicationCommand { get; private set; }

        [CommandCanExecuteSource(nameof(CurrentInstallation))]
        public ViewModelCommand<MainWindowViewModel> CancelOperationCommand { get; private set; }

        private readonly ObservableCollection<ApplicationViewModel> _Applications = new ObservableCollection<ApplicationViewModel>();
        public ObservableCollection<ApplicationViewModel> Applications
        {
            get
            {
                return _Applications;
            }
        }

        private void Applications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsDirty = true;

            if (e.NewItems != null)
                foreach (var newItem in e.NewItems)
                    if (newItem is ApplicationViewModel application)
                        application.PropertyChanged += Application_PropertyChanged;

            if (e.OldItems != null)
                foreach (var oldItem in e.OldItems)
                    if (oldItem is ApplicationViewModel application)
                        application.PropertyChanged -= Application_PropertyChanged;
        }

        private void Application_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                OnPropertyChanged(nameof(Applications));
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
                if (_CurrentInstallation != null)
                    _CurrentInstallation.PropertyChanged -= _CurrentInstallation_PropertyChanged;

                if (SetProperty(ref _CurrentInstallation, value))
                {
                    if (_CurrentInstallation != null)
                        _CurrentInstallation.PropertyChanged += _CurrentInstallation_PropertyChanged;

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

        private void _CurrentInstallation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                OnPropertyChanged(nameof(CurrentInstallation));
        }
    }
}
