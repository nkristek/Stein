using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using log4net;
using Ninject;
using Stein.Presentation;
using Stein.Services;
using Stein.ViewModels;

namespace Stein
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            // uncomment to test localization
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");

            Startup += App_Startup;

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        
        private void App_Startup(object sender, StartupEventArgs e)
        {
            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            // https://stackoverflow.com/a/520334
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // Initialize main window
            var rootWindow = new MainWindow();
            var kernel = new StandardKernel(new AppModule(rootWindow));
            
            try
            {
                kernel.Get<IConfigurationService>().LoadConfiguration();
            }
            catch (Exception exception)
            {
                Log.Error("Loading configuration failed, will create a new one", exception);
            }
            
            rootWindow.DataContext = kernel.Get<IViewModelService>().CreateViewModel<MainWindowViewModel>();
            rootWindow.Show();
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            DebugBreak();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception exception))
                return;

            Log.Error(exception);
            DebugBreak();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            DebugBreak();
        }

        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            DebugBreak();
        }

        [Conditional("DEBUG")]
        void DebugBreak()
        {
            if (Debugger.IsAttached)
                Debugger.Break();
        }
    }
}
