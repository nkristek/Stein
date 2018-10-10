using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NKristek.Smaragd.Commands;
using Stein.Presentation;
using Stein.Services;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public sealed class CustomOperationApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IInstallService _installService;

        private readonly IViewModelService _viewModelService;

        public CustomOperationApplicationCommand(ApplicationViewModel parent, IDialogService dialogService, IInstallService installService, IViewModelService viewModelService) 
            : base(parent)
        {
            _dialogService = dialogService;
            _installService = installService;
            _viewModelService = viewModelService;
        }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            if (!(viewModel.Parent is MainWindowViewModel mainWindowViewModel) || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any();
        }

        protected override async Task DoExecute(ApplicationViewModel viewModel, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null)
                return;

            try
            {
                mainWindowViewModel.CurrentInstallation = new InstallationViewModel();

                var installationResult = await PerformOperationsOfSelectedInstallerBundle(viewModel, mainWindowViewModel.CurrentInstallation);
                mainWindowViewModel.InstallationResult = installationResult.AnyOperationWasExecuted ? installationResult : null;
                if (installationResult.AnyOperationWasExecuted)
                    mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                _dialogService.ShowError(exception);
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }
            finally
            {
                mainWindowViewModel.CurrentInstallation = null;
            }
        }

        private async Task<InstallationResultViewModel> PerformOperationsOfSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
        {
            var installationResult = new InstallationResultViewModel();
            
            var dialogModel = _viewModelService.CreateViewModel<InstallerBundleDialogModel>(application.SelectedInstallerBundle);

            var dialogResult = _dialogService.ShowDialog(dialogModel);
            if (dialogResult != true)
                return installationResult;

            var installers = application.SelectedInstallerBundle.Installers
                .Where(i => i.PreferredOperation != InstallerOperation.DoNothing && !i.IsDisabled)
                .ToList();

            Log.Info($"Starting operation with {installers.Count} installers.");
            currentInstallation.InstallerCount = installers.Count;

            var logFolderPath = application.EnableInstallationLogging ? GetLogFileFolderPathForApplication(application.Name) : null;

            foreach (var installer in installers)
            {
                try
                {
                    if (currentInstallation.State == InstallationState.Cancelled)
                    {
                        Log.Info("Operation was cancelled.");
                        break;
                    }

                    currentInstallation.Name = installer.Name;
                    currentInstallation.CurrentIndex++;

                    var installLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(logFolderPath, installer.Name, "install") : null;
                    var uninstallLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(logFolderPath, installer.Name, "uninstall") : null;

                    switch (installer.PreferredOperation)
                    {
                        case InstallerOperation.Install:
                            currentInstallation.State = InstallationState.Install;
                            Log.Info($"Installing {installer.Name}.");

                            await _installService.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                            installationResult.InstallCount++;
                            break;
                        case InstallerOperation.Reinstall:
                            currentInstallation.State = InstallationState.Reinstall;
                            Log.Info($"Reinstalling {installer.Name}.");

                            // uninstall and install instead of reinstalling since the reinstall fails when another version of the installer was used (e.g. daily temps with the same version number)
                            await _installService.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);
                            
                            await _installService.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                            installationResult.ReinstallCount++;
                            break;
                        case InstallerOperation.Uninstall:
                            currentInstallation.State = InstallationState.Uninstall;
                            Log.Info($"Uninstalling {installer.Name}.");

                            await _installService.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);

                            installationResult.UninstallCount++;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    installationResult.FailedCount++;
                    Log.Error(exception);
                }
            }

            if (application.EnableInstallationLogging && application.AutomaticallyDeleteInstallationLogs)
            {
                try
                {
                    RemoveOldestFiles(logFolderPath, application.KeepNewestInstallationLogs);
                }
                catch (Exception exception)
                {
                    Log.Warn("Deleting old log files failed", exception);
                }
            }

            return installationResult;
        }

        private static void RemoveOldestFiles(string folderPath, int keepNewestLogFiles)
        {
            var logFolder = new DirectoryInfo(folderPath);
            foreach (var file in logFolder.EnumerateFiles().OrderByDescending(f => f.CreationTime).Skip(Math.Max(0, keepNewestLogFiles)))
            {
                try
                {
                    file.Delete();
                }
                catch (Exception exception)
                {
                    Log.Warn("Deleting log file failed", exception);
                }
            }
        }

        private static string GetLogFileFolderPathForApplication(string applicationName)
        {
            var logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein", "Logs", applicationName);
            if (!Directory.Exists(logFolderPath))
                Directory.CreateDirectory(logFolderPath);
            return logFolderPath;
        }

        private static string GetLogFilePathForInstaller(string applicationLogFolderName, string installerName, string installMethod)
        {
            var currentDate = DateTime.Now;
            var logFileName = $"{currentDate.Year}-{currentDate.Month}-{currentDate.Day}_{currentDate.Hour}-{currentDate.Minute}-{currentDate.Second}_{installerName}_{installMethod}.txt";
            var logFilePath = Path.Combine(applicationLogFolderName, logFileName);
            if (File.Exists(logFilePath))
                throw new IOException("File already exists");
            return logFilePath;
        }
    }
}
