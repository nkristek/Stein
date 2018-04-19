using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Stein.Services;
using Stein.Views;

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
            var appDataConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein");
            if (!Directory.Exists(appDataConfigurationPath))
                Directory.CreateDirectory(appDataConfigurationPath);
            
            ConfigurationService.Instance = new ConfigurationService(Path.Combine(appDataConfigurationPath, "Config.xml"));
            try
            {
                ConfigurationService.Instance.LoadConfigurationFromDisk();
            }
            catch (Exception exception)
            {
                Log.Error("Load configuration", exception);
            }

            InstallService.Instance = new InstallService();

            MsiService.Instance = new MsiService();

            ViewModelService.Instance = new ViewModelService();
            
            var rootWindow = new MainWindow();
            DialogService.Instance = new WpfDialogService(rootWindow);
            ProgressBarService.Instance = new TaskbarService(rootWindow);
            
            rootWindow.Show();
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("App_DispatcherUnhandledException", e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
                return;

            Log.Error("CurrentDomain_UnhandledException", exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Error("TaskScheduler_UnobservedTaskException", e.Exception);
        }

        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Error("WinFormApplication_ThreadException", e.Exception);
        }
    }
}
