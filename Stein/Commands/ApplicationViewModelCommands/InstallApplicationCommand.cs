using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;

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

                mainWindowViewModel.CurrentInstallation.State = InstallationViewModel.InstallationState.Install;
                mainWindowViewModel.CurrentInstallation.InstallerCount = installersToInstall.Count();
                
                foreach (var installer in installersToInstall)
                {
                    if (mainWindowViewModel.CurrentInstallation.State == InstallationViewModel.InstallationState.Cancelled)
                        break;

                    mainWindowViewModel.CurrentInstallation.Name = installer.Name;
                    mainWindowViewModel.CurrentInstallation.CurrentIndex++;

                    if (installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                        await InstallService.ReinstallAsync(installer.Path, viewModel.EnableSilentInstallation);
                    else
                        await InstallService.InstallAsync(installer.Path, viewModel.EnableSilentInstallation);

                    didInstall = true;
                }
            }

            mainWindowViewModel.CurrentInstallation = null;

            if (didInstall)
                mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
        }

        public override void OnThrownExeption(ApplicationViewModel viewModel, object view, object parameter, Exception exception)
        {
            MessageBox.Show(exception.Message);
            (viewModel.Parent as MainWindowViewModel)?.RefreshApplicationsCommand.ExecuteAsync(null).Wait();
        }
    }
}
