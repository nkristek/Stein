using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using NKristek.Smaragd.ViewModels;
using Stein.Presentation;
using Stein.ViewModels.Services;

namespace Stein.ViewModels.Commands.MainWindowViewModelCommands
{
    public sealed class RefreshApplicationsCommand
        : AsyncViewModelCommand2<MainWindowViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IViewModelService _viewModelService;

        public RefreshApplicationsCommand(IDialogService dialogService, IViewModelService viewModelService) 
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _viewModelService = viewModelService ?? throw new ArgumentNullException(nameof(viewModelService));
        }

        [CanExecuteSource(nameof(MainWindowViewModel.CurrentInstallation))]
        protected override bool CanExecute(MainWindowViewModel viewModel, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        protected override async Task ExecuteAsync(MainWindowViewModel viewModel, object parameter)
        {
            try
            {
                await _viewModelService.UpdateViewModelAsync(viewModel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
            }
        }
    }

    /// <inheritdoc cref="IViewModelCommand{TViewModel}" />
    /// <remarks>
    /// This defines an asynchronous command.
    /// </remarks>
    public abstract class AsyncViewModelCommand2<TViewModel>
        : Bindable, IViewModelCommand<TViewModel>, IAsyncCommand where TViewModel : class, IViewModel
    {
        private readonly IList<string> _cachedCanExecuteSourceNames;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncViewModelCommand{TViewModel}" /> class.
        /// </summary>
        protected AsyncViewModelCommand2()
        {
            _cachedCanExecuteSourceNames = GetCanExecuteSourceNames();
        }

        private IList<string> GetCanExecuteSourceNames()
        {
            var canExecuteMethods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m => m.Name == nameof(CanExecute));
            var canExecuteSourceAttributes = canExecuteMethods.SelectMany(m => m.GetCustomAttributes<CanExecuteSourceAttribute>());
            return canExecuteSourceAttributes.SelectMany(a => a.PropertySources).Distinct().ToList();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This defaults to the name of the type, including its namespace but not its assembly.
        /// </remarks>
        public virtual string Name => GetType().FullName;

        private WeakReference<TViewModel> _parent;

        /// <inheritdoc />
        public TViewModel Parent
        {
            get => _parent != null && _parent.TryGetTarget(out var parent) ? parent : null;
            set
            {
                if (value == Parent) return;
                var oldValue = Parent;
                if (oldValue != null)
                    oldValue.PropertyChanged -= ParentOnPropertyChanged;

                _parent = value != null ? new WeakReference<TViewModel>(value) : null;
                RaisePropertyChanged();

                if (value != null)
                    value.PropertyChanged += ParentOnPropertyChanged;
            }
        }

        private void ParentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_cachedCanExecuteSourceNames.Contains(e.PropertyName))
                RaiseCanExecuteChanged();
        }

        private bool _isWorking;

        /// <inheritdoc />
        public bool IsWorking
        {
            get => _isWorking;
            private set
            {
                if (SetProperty(ref _isWorking, value, out _))
                    RaiseCanExecuteChanged();
            }
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            return CanExecute(Parent, parameter);
        }

        /// <inheritdoc />
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(object parameter)
        {
            try
            {
                IsWorking = true;
                await ExecuteAsync(Parent, parameter);
            }
            finally
            {
                IsWorking = false;
            }
        }

        /// <inheritdoc cref="ICommand.CanExecute" />
        protected virtual bool CanExecute(TViewModel viewModel, object parameter)
        {
            return true;
        }

        /// <inheritdoc cref="IAsyncCommand.ExecuteAsync" />
        protected abstract Task ExecuteAsync(TViewModel viewModel, object parameter);

        /// <inheritdoc />
        public virtual event EventHandler CanExecuteChanged;

        /// <inheritdoc />
        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
