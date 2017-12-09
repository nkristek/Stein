using System.Collections.ObjectModel;
using System.ComponentModel;
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
        /// <summary>
        /// List of all applications
        /// </summary>
        public ObservableCollection<ApplicationViewModel> Applications
        {
            get
            {
                return _Applications;
            }
        }

        /// <summary>
        /// Attach property changed handler to elements in the collection to raise a PropertyChanged event if an element changed
        /// </summary>
        /// <param name="sender">The collection</param>
        /// <param name="e">EventArgs</param>
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

        /// <summary>
        /// Raise a PropertyChanged event for the collection if a property changed on the item of the collection
        /// </summary>
        /// <param name="sender">Item of the collection</param>
        /// <param name="e">EventArgs</param>
        private void Application_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                RaisePropertyChanged(nameof(Applications));
        }
        
        private InstallationViewModel _CurrentInstallation;
        /// <summary>
        /// Is set if an operation is in progress
        /// </summary>
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

        /// <summary>
        /// Raise a PropertyChanged event for the CurrentInstallation if a property changed on the InstallationViewModel
        /// </summary>
        /// <param name="sender">The InstallationViewModel</param>
        /// <param name="e">EventArgs</param>
        private void _CurrentInstallation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsDirty) && e.PropertyName != nameof(Parent) && e.PropertyName != nameof(View))
                RaisePropertyChanged(nameof(CurrentInstallation));
        }
    }
}
