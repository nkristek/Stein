using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class CustomOperationApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public CustomOperationApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
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
            
            var installationResult = await PerformOperationsOfSelectedInstallerBundle(viewModel, mainWindowViewModel.CurrentInstallation);
            if (installationResult.InstallCount > 0 
             || installationResult.ReinstallCount > 0 
             || installationResult.UninstallCount > 0 
             || installationResult.FailedCount > 0)
            {
                mainWindowViewModel.InstallationResult = installationResult;
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }
            
            mainWindowViewModel.CurrentInstallation = null;
        }

        private async Task<InstallationResultViewModel> PerformOperationsOfSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
        {
            var installationResult = new InstallationResultViewModel
            {
                Parent = currentInstallation.Parent
            };

            var dialogModel = new InstallerBundleDialogModel
            {
                Parent = application.SelectedInstallerBundle,
                Title = application.SelectedInstallerBundle.Name,
                Name = application.SelectedInstallerBundle.Name,
                Path = application.SelectedInstallerBundle.Path
            };
            foreach (var installer in application.SelectedInstallerBundle.Installers)
                dialogModel.Installers.Add(installer);

            var dialogResult = DialogService.Instance.ShowDialog(dialogModel);
            if (dialogResult != true)
                return installationResult;

            var installers = application.SelectedInstallerBundle.Installers
                .Where(i => i.PreferredOperation != InstallerOperationType.DoNothing && !i.IsDisabled)
                .ToList();

            await LogService.LogInfoAsync(String.Format("Starting operation with {0} installers.", installers.Count));
            currentInstallation.InstallerCount = installers.Count;
            
            foreach (var installer in installers)
            {
                try
                {
                    if (currentInstallation.State == InstallationState.Cancelled)
                    {
                        await LogService.LogInfoAsync("Operation was cancelled.");
                        break;
                    }

                    currentInstallation.Name = installer.Name;
                    currentInstallation.CurrentIndex++;

                    var installLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(installer.Name, "install") : null;
                    var uninstallLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(installer.Name, "uninstall") : null;

                    switch (installer.PreferredOperation)
                    {
                        case InstallerOperationType.Install:
                            currentInstallation.State = InstallationState.Install;
                            await LogService.LogInfoAsync(String.Format("Installing {0}.", installer.Name));
                            
                            await InstallService.Instance.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                            installationResult.InstallCount++;
                            break;
                        case InstallerOperationType.Reinstall:
                            currentInstallation.State = InstallationState.Reinstall;
                            await LogService.LogInfoAsync(String.Format("Reinstalling {0}.", installer.Name));

                            // uninstall and install instead of reinstalling since the reinstall fails when another version of the installer was used (e.g. daily temps with the same version number)
                            await InstallService.Instance.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);
                            
                            await InstallService.Instance.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                            installationResult.ReinstallCount++;
                            break;
                        case InstallerOperationType.Uninstall:
                            currentInstallation.State = InstallationState.Uninstall;
                            await LogService.LogInfoAsync(String.Format("Uninstalling {0}.", installer.Name));

                            await InstallService.Instance.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);

                            installationResult.UninstallCount++;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    installationResult.FailedCount++;
                    await LogService.LogErrorAsync(exception);
                }
            }

            return installationResult;
        }

        private string GetLogFilePathForInstaller(string installerName, string installMethod)
        {
            var installLogFolderPath = Path.Combine(LogService.LogFolderPath, "Installs");
            var currentDate = DateTime.Now;
            var logFileName = String.Format("{0}_{1}_{2}-{3}-{4}_{5}-{6}-{7}.txt", installerName, installMethod, currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
            return Path.Combine(installLogFolderPath, logFileName);
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.Instance.ShowError(exception);

            var mainViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainViewModel == null)
                return;

            mainViewModel.CurrentInstallation = null;
            mainViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
