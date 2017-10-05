using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TempManager.Services;
using TempManager.ViewModels;
using WpfBase.Commands;

namespace TempManager.Commands.ApplicationViewModelCommands
{
    public class UninstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public UninstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            if (viewModel == null)
                return false;

            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>(); ;
            if (mainViewModel == null)
                return false;

            return mainViewModel.CurrentInstallation == null && viewModel.InstallerBundles.Any() && viewModel.InstallerBundles.Any(ib => ib.Installers.Any(i => i.IsInstalled));
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var installerBundle = viewModel.InstallerBundles.LastOrDefault(ib => ib.Installers.Any(i => i.IsInstalled));
            var installers = installerBundle.Installers.Where(i => i.IsInstalled);

            var installerCount = installers.Count();
            var currentInstaller = 0;

            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            mainViewModel.CurrentInstallation = new InstallationViewModel()
            {
                Type = InstallationViewModel.InstallationType.Uninstall
            };

            foreach (var installer in installers)
            {
                mainViewModel.CurrentInstallation.Name = installer.Name;
                mainViewModel.CurrentInstallation.Progress = ((currentInstaller * 100) / installerCount);
                currentInstaller++;

                Debug.WriteLine("Uninstalling " + installer.Name);
                await InstallService.UninstallAsync(installer);
            }
            
            mainViewModel.CurrentInstallation = null;
        }
    }
}
