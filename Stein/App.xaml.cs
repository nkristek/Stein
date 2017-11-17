using Stein.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
            Startup += App_Startup;
            Exit += App_Exit;

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException;
        }
        
        private void App_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ConfigurationService.LoadConfigurationFromDisk();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            ConfigurationService.SaveConfigurationToDisk();
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ProcessError(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
                return;
            
            ProcessError(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ProcessError(e.Exception);
        }

        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ProcessError(e.Exception);
        }

        private void ProcessError(Exception exception)
        {
            try
            {
                var errorBuilder = new StringBuilder();
                errorBuilder.Append(DateTime.Now.ToString());
                errorBuilder.Append(": ");
                errorBuilder.Append(exception.Message);

                var innerException = exception.InnerException;
                while (innerException != null)
                {
                    errorBuilder.AppendLine(innerException.Message);
                    innerException = innerException.InnerException;
                }

                var logFileName = String.Format("log-{0}.txt", DateTime.Now.ToShortDateString());
                var logFilePath = Path.Combine(ConfigurationService.ConfigurationFolderPath, logFileName);

                using (var file = File.AppendText(logFilePath))
                    file.WriteLine(errorBuilder.ToString());
            }
            catch { }
        }
    }
}
