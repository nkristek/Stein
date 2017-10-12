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

            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            return mainViewModel != null && mainViewModel.CurrentInstallation == null && viewModel.InstallerBundles.Any() && !viewModel.InstallerBundles.Any(ib => ib.Installers.Any(i => i.IsInstalled));
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            mainViewModel.CurrentInstallation = new InstallationViewModel(mainViewModel)
            {
                Type = InstallationViewModel.InstallationType.Install,
                InstallerCount = 0,
                CurrentIndex = 0
            };
            
            if (ViewModelService.ShowDialog(viewModel.SelectedInstallerBundle) == true)
            {
                var installersToInstall = viewModel.SelectedInstallerBundle.Installers.Where(i => i.IsEnabled && !i.IsInstalled);
                mainViewModel.CurrentInstallation.InstallerCount = installersToInstall.Count();

                var currentInstaller = 1;
                foreach (var installer in installersToInstall)
                {
                    mainViewModel.CurrentInstallation.Name = installer.Name;
                    mainViewModel.CurrentInstallation.CurrentIndex = currentInstaller;

                    Debug.WriteLine("Installing " + installer.Name);
                    await InstallService.InstallAsync(installer, viewModel.EnableSilentInstallation);
                    currentInstaller++;
                }
            }

            mainViewModel.CurrentInstallation = null;
            mainViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
