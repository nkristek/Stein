using System;
using System.Linq;
using System.Threading.Tasks;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class CustomOperationApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public CustomOperationApplicationCommand(ApplicationViewModel parent) : base(parent) { }

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

        private static async Task<InstallationResultViewModel> PerformOperationsOfSelectedInstallerBundle(ApplicationViewModel application, InstallationViewModel currentInstallation)
        {
            var installationResult = new InstallationResultViewModel(currentInstallation.Parent);

            var dialogResult = DialogService.ShowDialog(application.SelectedInstallerBundle, application.SelectedInstallerBundle.Name);
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

                    var installLogFilePath = application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_install")) : null;
                    var uninstallLogFilePath = application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_uninstall")) : null;

                    switch (installer.PreferredOperation)
                    {
                        case InstallerOperationType.Install:
                            currentInstallation.State = InstallationState.Install;
                            await LogService.LogInfoAsync(String.Format("Installing {0}.", installer.Name));
                            
                            await InstallService.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                            installationResult.InstallCount++;
                            break;
                        case InstallerOperationType.Reinstall:
                            currentInstallation.State = InstallationState.Reinstall;
                            await LogService.LogInfoAsync(String.Format("Reinstalling {0}.", installer.Name));

                            // uninstall and install instead of reinstalling since the reinstall fails when another version of the installer was used (e.g. daily temps with the same version number)
                            await InstallService.UninstallAsync(installer.ProductCode, application.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_uninstall")) : null, application.EnableSilentInstallation);
                            
                            await InstallService.InstallAsync(installer.Path, installLogFilePath, application.EnableSilentInstallation);

                            installationResult.ReinstallCount++;
                            break;
                        case InstallerOperationType.Uninstall:
                            currentInstallation.State = InstallationState.Uninstall;
                            await LogService.LogInfoAsync(String.Format("Uninstalling {0}.", installer.Name));

                            await InstallService.UninstallAsync(installer.ProductCode, uninstallLogFilePath, application.EnableSilentInstallation);

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
