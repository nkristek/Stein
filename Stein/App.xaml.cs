using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Stein.Services;
using Stein.ViewModels;
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
            var mainWindow = new MainWindow();

            var dialogService = new WpfDialogService(mainWindow);
            var configurationService = new ConfigurationService(GetConfigurationFilePath());
            try
            {
                configurationService.LoadConfiguration();
            }
            catch (Exception exception)
            {
                Log.Error("Load configuration", exception);
                dialogService.ShowError(exception);
            }
            var installService = new InstallService();
            var viewModelService = new ViewModelService(dialogService, configurationService, installService);
            var themeService = new WpfThemeService();
            var progressBarService = new WpfTaskbarService(mainWindow);
            var msiService = new MsiService();
            var mainWindowViewModel = new MainWindowViewModel(dialogService, viewModelService, themeService, progressBarService, configurationService, installService, msiService);

            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();
        }

        private static string GetConfigurationFilePath()
        {
            var appDataConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein");
            if (!Directory.Exists(appDataConfigurationPath))
                Directory.CreateDirectory(appDataConfigurationPath);
            return Path.Combine(appDataConfigurationPath, "Config.xml");
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
