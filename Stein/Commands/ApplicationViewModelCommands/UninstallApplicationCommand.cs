using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class UninstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public UninstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any(i => i.IsInstalled == true);
        }
        
        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null)
                return;

            mainWindowViewModel.CurrentInstallation = new InstallationViewModel(mainWindowViewModel)
            {
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

        private static async Task<InstallationResultViewModel> UninstallSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
        {
            var installationResult = new InstallationResultViewModel(currentInstallation.Parent);

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

                    var uninstallLogFilePath = application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_uninstall")) : null;
                    await InstallService.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);

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

        protected override void OnThrownException(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            DialogService.ShowErrorDialog(exception);

            var mainViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainViewModel == null)
                return;

            mainViewModel.CurrentInstallation = null;
            mainViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
