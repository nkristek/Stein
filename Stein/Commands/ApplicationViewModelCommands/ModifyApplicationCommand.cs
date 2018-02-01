using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using nkristek.MVVMBase.Commands;
using nkristek.Stein.Localizations;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein.Commands.ApplicationViewModelCommands
{
    public class ModifyApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public ModifyApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        protected override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.FirstParentOfType<MainWindowViewModel>();
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;

            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any();
        }

        protected override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.FirstParentOfType<MainWindowViewModel>();
            if (mainWindowViewModel == null)
                return;

            mainWindowViewModel.CurrentInstallation = new InstallationViewModel(mainWindowViewModel)
            {
                State = InstallationState.Preparing,
                InstallerCount = 0,
                CurrentIndex = 0
            };

            foreach (var installer in viewModel.SelectedInstallerBundle.Installers)
                installer.PreferredOperation = InstallerOperationType.DoNothing;

            var didInstallCount = 0;
            var didReinstallCount = 0;
            var didUninstallCount = 0;
            var didFailedCount = 0;

            if (DialogService.ShowDialog(viewModel.SelectedInstallerBundle, viewModel.SelectedInstallerBundle.Name) == true)
            {
                var installers = viewModel.SelectedInstallerBundle.Installers.Where(i => i.PreferredOperation != InstallerOperationType.DoNothing && !i.IsDisabled).ToList();

                await LogService.LogInfoAsync(String.Format("Starting operation with {0} installers.", installers.Count));

                mainWindowViewModel.CurrentInstallation.InstallerCount = installers.Count;

                foreach (var installer in installers)
                {
                    try
                    {
                        if (mainWindowViewModel.CurrentInstallation.State == InstallationState.Cancelled)
                        {
                            await LogService.LogInfoAsync("Operation was cancelled.");
                            break;
                        }

                        mainWindowViewModel.CurrentInstallation.Name = installer.Name;
                        mainWindowViewModel.CurrentInstallation.CurrentIndex++;

                        switch (installer.PreferredOperation)
                        {
                            case InstallerOperationType.Install:
                                mainWindowViewModel.CurrentInstallation.State = InstallationState.Install;
                                await LogService.LogInfoAsync(String.Format("Installing {0}.", installer.Name));
                                await InstallService.InstallAsync(installer.Path, viewModel.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_install")) : null, viewModel.EnableSilentInstallation);

                                didInstallCount++;

                                break;
                            case InstallerOperationType.Reinstall:
                                mainWindowViewModel.CurrentInstallation.State = InstallationState.Reinstall;
                                await LogService.LogInfoAsync(String.Format("Reinstalling {0}.", installer.Name));
                                // uninstall and install instead of reinstalling since the reinstall fails when another version of the installer was used (e.g. daily temps with the same version number)
                                if (installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                                    await InstallService.UninstallAsync(installer.ProductCode, viewModel.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_uninstall")) : null, viewModel.EnableSilentInstallation);
                                await InstallService.InstallAsync(installer.Path, viewModel.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_install")) : null, viewModel.EnableSilentInstallation);

                                didReinstallCount++;

                                break;
                            case InstallerOperationType.Uninstall:
                                mainWindowViewModel.CurrentInstallation.State = InstallationState.Uninstall;
                                await LogService.LogInfoAsync(String.Format("Uninstalling {0}.", installer.Name));
                                if (installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                                    await InstallService.UninstallAsync(installer.ProductCode, viewModel.EnableInstallationLogging ? InstallService.GetLogFilePathForInstaller(String.Concat(installer.Name, "_uninstall")) : null, viewModel.EnableSilentInstallation);

                                didUninstallCount++;

                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        didFailedCount++;
                        MessageBox.Show(exception.Message);
                    }
                }
            }

            foreach (var installer in viewModel.SelectedInstallerBundle.Installers)
                installer.PreferredOperation = InstallerOperationType.DoNothing;

            mainWindowViewModel.CurrentInstallation = null;

            if (didInstallCount > 0 || didReinstallCount > 0 || didUninstallCount > 0 || didFailedCount > 0)
            {
                var resultMessage = String.Format(Strings.DidInstallXPrograms, didInstallCount, didReinstallCount, didUninstallCount);
                if (didFailedCount > 0)
                    resultMessage = String.Join("\n", resultMessage, String.Format(Strings.XInstallersFailed, didFailedCount));

                MessageBox.Show(resultMessage);
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
            }
        }

        protected override void OnThrownException(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);

            var mainViewModel = viewModel.FirstParentOfType<MainWindowViewModel>();
            if (mainViewModel == null)
                return;

            mainViewModel.CurrentInstallation = null;
            mainViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
