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
            Startup += new StartupEventHandler(App_Startup);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        
        void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
        }

        void ProcessError(Exception exception)
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

        void ShowError(Exception exception)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Unhandled exception:");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine(exception.Message);
            MessageBox.Show(messageBuilder.ToString());
        }
        
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ShowError(e.Exception);
            ProcessError(e.Exception);
            e.Handled = true;
        }
        
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
                return;

            ShowError(exception);
            ProcessError(exception);
        }
        
        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowError(e.Exception);
            ProcessError(e.Exception);

            e.SetObserved();
        }
        
        void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ShowError(e.Exception);
            ProcessError(e.Exception);

            ProcessError(e.Exception);
        }
    }
}
