using Stein.Services;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Stein
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // uncomment to test localization
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");

            Startup += App_Startup;
            Exit += App_Exit;

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException;

        }
        
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var appDataConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein");
            ConfigurationService.ConfiguationFolderPath = appDataConfigurationPath;
            var logFolderPath = Path.Combine(appDataConfigurationPath, "Logs");
            LogService.LogFolderPath = logFolderPath;
            InstallService.InstallationLogFolderPath = Path.Combine(logFolderPath, "Installs");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            ConfigurationService.LoadConfigurationFromDisk();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            ConfigurationService.SaveConfigurationToDisk();
            LogService.CloseLogFiles();
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogService.LogError(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
                return;

            LogService.LogError(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogService.LogError(e.Exception);
        }

        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogService.LogError(e.Exception);
        }
    }
}
