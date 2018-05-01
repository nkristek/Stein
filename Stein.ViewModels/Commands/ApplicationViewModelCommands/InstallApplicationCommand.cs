﻿using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.ViewModels.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Stein.Presentation;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class InstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDialogService _dialogService;

        private readonly IInstallService _installService;

        public InstallApplicationCommand(ApplicationViewModel parent, IDialogService dialogService, IInstallService installService) : base(parent)
        {
            _dialogService = dialogService;
            _installService = installService;
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

            mainWindowViewModel.CurrentInstallation = new InstallationViewModel
            {
                Parent = mainWindowViewModel,
                State = InstallationState.Preparing,
                InstallerCount = 0,
                CurrentIndex = 0
            };

            var installationResult = await InstallSelectedInstallerBundle(viewModel, mainWindowViewModel.CurrentInstallation);
            if (installationResult.InstallCount > 0 || installationResult.ReinstallCount > 0 || installationResult.FailedCount > 0)
            {
                mainWindowViewModel.InstallationResult = installationResult;
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }

            mainWindowViewModel.CurrentInstallation = null;
        }

        private async Task<InstallationResultViewModel> InstallSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
        {
            var installationResult = new InstallationResultViewModel
            {
                Parent = currentInstallation.Parent
            };

            // filter installers with the same name 
            // if no name is set don't filter by grouping by path (which will always be distinct)
            var installers = application.SelectedInstallerBundle.Installers
                .Where(i => !i.IsDisabled)
                .GroupBy(i => !String.IsNullOrEmpty(i.Name) ? i.Name : i.Path).Select(ig => ig.First())
                .ToList();

            Log.Info($"Starting install operation with {installers.Count} installers.");
            currentInstallation.InstallerCount = installers.Count;
            
            foreach (var installer in installers)
            {
                try
                {
                    if (currentInstallation.State == InstallationState.Cancelled)
                    {
                        Log.Info("Install operation was cancelled.");
                        break;
                    }

                    currentInstallation.Name = installer.Name;
                    currentInstallation.CurrentIndex++;

                    if (installer.IsInstalled == false)
                    {
                        currentInstallation.State = InstallationState.Install;
                        Log.Info($"Installing {installer.Name}.");

                        var logFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(application.Name, installer.Name, "install") : null;
                        await _installService.InstallAsync(installer.Path, logFilePath, application.EnableSilentInstallation);

                        installationResult.InstallCount++;
                    }
                    else
                    {
                        if (installer.IsInstalled == null)
                            Log.Info("There is no information if the installer is already installed, trying to reinstall.");

                        currentInstallation.State = InstallationState.Reinstall;
                        Log.Info($"Reinstalling {installer.Name}.");

                        // uninstall and install instead of reinstalling since the reinstall fails when another version of the installer was used (e.g. daily temps with the same version number)
                        if (installer.IsInstalled == null || installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                        {
                            var uninstallLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(application.Name, installer.Name, "uninstall") : null;
                            await _installService.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);
                        }
                        var installLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(application.Name, installer.Name, "install") : null;
                        await _installService.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                        installationResult.ReinstallCount++;
                    }
                }
                catch (Exception exception)
                {
                    installationResult.FailedCount++;
                    Log.Error(exception);
                }
            }

            return installationResult;
        }

        private static string GetLogFilePathForInstaller(string applicationName, string installerName, string installMethod)
        {
            var logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein", "Logs", applicationName);
            if (!Directory.Exists(logFolderPath))
                Directory.CreateDirectory(logFolderPath);

            var currentDate = DateTime.Now;
            var logFileName = $"{currentDate.Year}-{currentDate.Month}-{currentDate.Day}_{currentDate.Hour}-{currentDate.Minute}-{currentDate.Second}_{installerName}_{installMethod}.txt";
            var logFilePath = Path.Combine(logFolderPath, logFileName);
            if (File.Exists(logFilePath))
                throw new IOException("File already exists");

            return logFilePath;
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object parameter, Exception exception)
        {
            Log.Error(exception);
            _dialogService.ShowError(exception);

            if (!(viewModel.Parent is MainWindowViewModel mainViewModel))
                return;

            mainViewModel.CurrentInstallation = null;
            mainViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
