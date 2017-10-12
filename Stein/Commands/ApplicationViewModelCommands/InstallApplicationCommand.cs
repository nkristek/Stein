using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;

namespace Stein.Commands.ApplicationViewModelCommands
{
    public class InstallApplicationCommand
        :  AsyncViewModelCommand<ApplicationViewModel>
    {
        public InstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            if (viewModel.SelectedInstallerBundle == null)
                return false;

            var mainWindowViewModel = viewModel.FirstParentOfType<MainWindowViewModel>();
            return mainWindowViewModel != null && mainWindowViewModel.CurrentInstallation == null && viewModel.InstallerBundles.Any() && !viewModel.InstallerBundles.Any(ib => ib.Installers.Any(i => i.IsInstalled));
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.FirstParentOfType<MainWindowViewModel>();
            mainWindowViewModel.CurrentInstallation = new InstallationViewModel(mainWindowViewModel)
            {
                Type = InstallationViewModel.InstallationType.Install,
                InstallerCount = 0,
                CurrentIndex = 0
            };
            
            if (ViewModelService.ShowDialog(viewModel.SelectedInstallerBundle) == true)
            {
                var installersToInstall = viewModel.SelectedInstallerBundle.Installers.Where(i => i.IsEnabled && !i.IsInstalled);
                mainWindowViewModel.CurrentInstallation.InstallerCount = installersToInstall.Count();

                var currentInstaller = 1;
                foreach (var installer in installersToInstall)
                {
                    mainWindowViewModel.CurrentInstallation.Name = installer.Name;
                    mainWindowViewModel.CurrentInstallation.CurrentIndex = currentInstaller;

                    Debug.WriteLine("Installing " + installer.Name);
                    await InstallService.InstallAsync(installer, viewModel.EnableSilentInstallation);
                    currentInstaller++;
                }
            }

            mainWindowViewModel.CurrentInstallation = null;
            mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
