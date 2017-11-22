using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class UninstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public UninstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;
            
            return viewModel.SelectedInstallerBundle != null && viewModel.SelectedInstallerBundle.Installers.Any(i => i.IsInstalled.HasValue && i.IsInstalled.Value);
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

            var didUninstall = false;

            // disable installers which are not installed
            foreach (var installer in viewModel.SelectedInstallerBundle.Installers)
                installer.IsDisabled = installer.IsInstalled.HasValue && !installer.IsInstalled.Value;
            
            if (DialogService.ShowDialog(viewModel.SelectedInstallerBundle, "Select installers") == true)
            {
                var installersToInstall = viewModel.SelectedInstallerBundle.Installers.Where(i => i.IsEnabled && !i.IsDisabled);

                mainWindowViewModel.CurrentInstallation.State = InstallationViewModel.InstallationState.Uninstall;
                mainWindowViewModel.CurrentInstallation.InstallerCount = installersToInstall.Count();
                
                foreach (var installer in installersToInstall)
                {
                    if (mainWindowViewModel.CurrentInstallation.State == InstallationViewModel.InstallationState.Cancelled)
                        break;

                    mainWindowViewModel.CurrentInstallation.Name = installer.Name;
                    mainWindowViewModel.CurrentInstallation.CurrentIndex++;
                    
                    await InstallService.UninstallAsync(installer.Path, viewModel.EnableSilentInstallation);
                    didUninstall = true;
                }
            }

            // reenable previously disabled installers
            foreach (var installer in viewModel.SelectedInstallerBundle.Installers)
                installer.IsDisabled = false;

            mainWindowViewModel.CurrentInstallation = null;

            if (didUninstall)
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
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
