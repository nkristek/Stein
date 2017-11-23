using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using System.IO;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class InstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public InstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;
            
            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any();
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            mainWindowViewModel.CurrentInstallation = new InstallationViewModel(mainWindowViewModel)
            {
                State = InstallationViewModel.InstallationState.Preparing,
                InstallerCount = 0,
                CurrentIndex = 0
            };

            var didInstall = false;

            if (DialogService.ShowDialog(viewModel.SelectedInstallerBundle, "Select installers") == true)
            {
                var installersToInstall = viewModel.SelectedInstallerBundle.Installers.Where(i => i.IsEnabled && !i.IsDisabled);
                var installerCount = installersToInstall.Count();

                await LogService.LogInfoAsync(String.Format("Installing {0} installers.", installerCount));
                
                mainWindowViewModel.CurrentInstallation.InstallerCount = installersToInstall.Count();
                
                foreach (var installer in installersToInstall)
                {
                    if (mainWindowViewModel.CurrentInstallation.State == InstallationViewModel.InstallationState.Cancelled)
                    {
                        await LogService.LogInfoAsync("Operation was cancelled.");
                        break;
                    }

                    mainWindowViewModel.CurrentInstallation.Name = installer.Name;
                    mainWindowViewModel.CurrentInstallation.CurrentIndex++;

                    if (installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                    {
                        mainWindowViewModel.CurrentInstallation.State = InstallationViewModel.InstallationState.Reinstall;
                        await LogService.LogInfoAsync(String.Format("Reinstalling {0}.", installer.Name));
                        await InstallService.ReinstallAsync(installer.ProductCode, installer.Path, viewModel.EnableInstallationLogging ? GetLogFilePathForInstaller(installer) : null, viewModel.EnableSilentInstallation);
                    }
                    else
                    {
                        mainWindowViewModel.CurrentInstallation.State = InstallationViewModel.InstallationState.Install;
                        await LogService.LogInfoAsync(String.Format("Installing {0}.", installer.Name));
                        await InstallService.InstallAsync(installer.Path, viewModel.EnableInstallationLogging ? GetLogFilePathForInstaller(installer) : null, viewModel.EnableSilentInstallation);
                    }

                    didInstall = true;
                }
            }

            mainWindowViewModel.CurrentInstallation = null;

            if (didInstall)
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
        }

        private static string GetLogFilePathForInstaller(InstallerViewModel installer)
        {
            var currentDate = DateTime.Now;
            var logFileName = String.Format("{0}_{1}-{2}-{3}_{4}-{5}-{6}.txt", installer.Name, currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
            return Path.Combine(InstallService.InstallationLogFolderPath, logFileName);
        }

        public override void OnThrownExeption(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            LogService.LogError(exception);
            MessageBox.Show(exception.Message);
            Task.Run(async () =>
            {
                await (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.ExecuteAsync(null);
            });
        }
    }
}
