using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using log4net;
using Ninject;
using Stein.Presentation;
using Stein.Services.Configuration;
using Stein.ViewModels;
using Stein.ViewModels.Commands.MainWindowDialogModelCommands;
using Stein.ViewModels.Extensions;

namespace Stein
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IViewModelService _viewModelService;

        private IDialogService _dialogService;
        
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            // https://stackoverflow.com/a/520334
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            var kernel = new StandardKernel(new AppModule());
            _dialogService = kernel.Get<IDialogService>();
            _viewModelService = kernel.Get<IViewModelService>();

            var configurationService = kernel.Get<IConfigurationService>();
            try
            {
                await configurationService.LoadConfigurationAsync();
            }
            catch (Exception exception)
            {
                // TODO GitHub issue #27: show welcome view
                Log.Error("Loading configuration failed, will create a new one", exception);
            }

            var themeService = kernel.Get<IThemeService>();
            themeService.SetTheme(configurationService.Configuration.SelectedTheme);
            
            var viewModel = _viewModelService.CreateViewModel<MainWindowDialogModel>();
            viewModel.GetCommand<MainWindowDialogModel, RefreshApplicationsCommand>()?.ExecuteAsync(null);
            _dialogService.Show(viewModel);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception;
            HandleException(exception);

            e.Handled = true;
        }
        
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception exception))
                return;

            HandleException(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception;
            HandleException(exception);

            if (!e.Observed)
                e.SetObserved();
        }

        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var exception = e.Exception;
            HandleException(exception);
        }

        private void HandleException(Exception exception)
        {
            try
            {
                Log.Error(exception);
                ShowExceptionDialog(exception);
            }
            catch
            {
                // no further escalation
            }
        }

        private bool? ShowExceptionDialog(Exception exception)
        {
            return Dispatcher.Invoke(() =>
            {
                if (_viewModelService == null || _dialogService == null)
                {
                    MessageBox.Show(exception.ToString(), "Error");
                    Environment.Exit(1);
                }
                var exceptionDialogModel = _viewModelService.CreateViewModel<ExceptionDialogModel>(null, exception);
                return _dialogService.ShowDialog(exceptionDialogModel);
            });
        }
    }
}
