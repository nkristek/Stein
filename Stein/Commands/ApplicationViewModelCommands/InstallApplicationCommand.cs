using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class InstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public InstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any();
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

            var installationResult = await InstallSelectedInstallerBundle(viewModel, mainWindowViewModel.CurrentInstallation);
            if (installationResult.InstallCount > 0 || installationResult.ReinstallCount > 0 || installationResult.FailedCount > 0)
            {
                mainWindowViewModel.InstallationResult = installationResult;
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }

            mainWindowViewModel.CurrentInstallation = null;
        }

        private static async Task<InstallationResultViewModel> InstallSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
        {
            var installationResult = new InstallationResultViewModel(currentInstallation.Parent);

            // filter installers with the same name 
            // if no name is set don't filter by grouping by path (which will always be distinct)
            var installers = application.SelectedInstallerBundle.Installers
                .Where(i => !i.IsDisabled)
                .GroupBy(i => !String.IsNullOrEmpty(i.Name) ? i.Name : i.Path).Select(ig => ig.First())
                .ToList();

            await LogService.LogInfoAsync(String.Format("Starting install operation with {0} installers.", installers.Count));
            currentInstallation.InstallerCount = installers.Count;
            
            foreach (var installer in installers)
            {
                try
                {
                    if (currentInstallation.State == InstallationState.Cancelled)
                    {
                        await LogService.LogInfoAsync("Install operation was cancelled.");
                        break;
                    }

                    currentInstallation.Name = installer.Name;
                    currentInstallation.CurrentIndex++;

                    if (installer.IsInstalled == false)
                    {
                        currentInstallation.State = InstallationState.Install;
                        await LogService.LogInfoAsync(String.Format("Installing {0}.", installer.Name));

                        var logFilePath = application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_install")) : null;
                        await InstallService.InstallAsync(installer.Path, logFilePath, application.EnableSilentInstallation);

                        installationResult.InstallCount++;
                    }
                    else
                    {
                        if (installer.IsInstalled == null)
                            await LogService.LogInfoAsync("There is no information if the installer is already installed, trying to reinstall.");

                        currentInstallation.State = InstallationState.Reinstall;
                        await LogService.LogInfoAsync(String.Format("Reinstalling {0}.", installer.Name));

                        // uninstall and install instead of reinstalling since the reinstall fails when another version of the installer was used (e.g. daily temps with the same version number)
                        if (installer.IsInstalled == null || installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                        {
                            var uninstallLogFilePath = application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_uninstall")) : null;
                            await InstallService.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);
                        }
                        var installLogFilePath = application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_install")) : null;
                        await InstallService.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                        installationResult.ReinstallCount++;
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
