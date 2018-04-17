using nkristek.MVVMBase.Commands;
using Stein.Services;
using Stein.ViewModels.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stein.ViewModels.Commands.ApplicationViewModelCommands
{
    public class UninstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public UninstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any(i => i.IsInstalled == true);
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
            
            var installationResult = await UninstallSelectedInstallerBundle(viewModel, mainWindowViewModel.CurrentInstallation);
            if (installationResult.UninstallCount > 0 || installationResult.FailedCount > 0)
            {
                mainWindowViewModel.InstallationResult = installationResult;
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }

            mainWindowViewModel.CurrentInstallation = null;
        }

        private async Task<InstallationResultViewModel> UninstallSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
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

            await LogService.LogInfoAsync(String.Format("Starting uninstall operation with {0} installers.", installers.Count));
            currentInstallation.InstallerCount = installers.Count;
            
            foreach (var installer in installers)
            {
                try
                {
                    if (currentInstallation.State == InstallationState.Cancelled)
                    {
                        await LogService.LogInfoAsync("Uninstall operation was cancelled.");
                        break;
                    }

                    currentInstallation.Name = installer.Name;
                    currentInstallation.CurrentIndex++;

                    if (installer.IsInstalled == false)
                        continue;

                    if (installer.IsInstalled == null)
                        await LogService.LogInfoAsync("There is no information if the installer is already installed, trying to uninstall.");

                    currentInstallation.State = InstallationState.Uninstall;
                    await LogService.LogInfoAsync(String.Format("Uninstalling {0}.", installer.Name));

                    var uninstallLogFilePath = application.EnableInstallationLogging ? GetLogFilePathForInstaller(installer.Name, "uninstall") : null;
                    await InstallService.Instance.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);

                    installationResult.UninstallCount++;
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
