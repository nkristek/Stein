using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using log4net;
using Ninject;
using Ninject.Parameters;
using Stein.Presentation;
using Stein.Services.Configuration;
using Stein.ViewModels;
using Stein.ViewModels.Commands.MainWindowViewModelCommands;
using Stein.ViewModels.Extensions;
using Stein.ViewModels.Services;

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
            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            // https://stackoverflow.com/a/520334
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            var mainWindow = new MainWindow();
            var kernel = new StandardKernel(new AppModule(mainWindow));

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

            _viewModelService = kernel.Get<IViewModelService>();
            _dialogService = kernel.Get<IDialogService>();

            var viewModel = _viewModelService.CreateViewModel<MainWindowViewModel>();
            viewModel.GetCommand<MainWindowViewModel, RefreshApplicationsCommand>()?.ExecuteAsync(null);
            mainWindow.DataContext = viewModel;

            mainWindow.Show();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception;
            Log.Error(exception);
            ShowExceptionDialog(exception);

#if DEBUG
            if (Debugger.IsAttached)
                Debugger.Break();
#endif

            e.Handled = true;
        }
        
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception exception))
                return;
            
            Log.Error(exception);
            ShowExceptionDialog(exception);
#if DEBUG
            if (Debugger.IsAttached)
                Debugger.Break();
#endif
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception;
            Log.Error(exception);
            ShowExceptionDialog(exception);

#if DEBUG
            if (Debugger.IsAttached)
                Debugger.Break();
#endif

            if (!e.Observed)
                e.SetObserved();
        }

        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var exception = e.Exception;
            Log.Error(exception);
            ShowExceptionDialog(exception);

#if DEBUG
            if (Debugger.IsAttached)
                Debugger.Break();
#endif
        }

        private bool? ShowExceptionDialog(Exception exception)
        {
            var exceptionDialogModel = _viewModelService.CreateViewModel<ExceptionDialogModel>(null, exception);
            return _dialogService.ShowDialog(exceptionDialogModel);
        }
    }
}
