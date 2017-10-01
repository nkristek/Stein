using System;
using System.Collections.Generic;
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
    public class InstallApplicationCommand
        :  AsyncViewModelCommand<ApplicationViewModel>
    {
        public InstallApplicationCommand(ApplicationViewModel parent, object view = null) : base(parent, view) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            if (viewModel == null)
                return false;

            if (viewModel.SelectedInstallerBundle == null)
                return false;
            
            return viewModel.InstallerBundles.Any() && !viewModel.InstallerBundles.Any(ib => ib.Installers.Any(i => i.IsInstalled));
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainViewModel = viewModel.Parent as MainViewModel;
            if (mainViewModel == null)
                return;

            var installers = viewModel.SelectedInstallerBundle.Installers;

            var installerCount = installers.Count();
            var currentInstaller = 0;

            foreach (var installer in installers)
            {
                mainViewModel.CurrentInstallationName = installer.Name;
                mainViewModel.CurrentInstallationProgress = ((currentInstaller * 100) / (installerCount));
                currentInstaller++;

                Debug.WriteLine("Installing " + installer.Name);
                await InstallService.Install(installer);
            }

            mainViewModel.CurrentInstallationName = null;
            mainViewModel.CurrentInstallationProgress = null;
        }
    }
}
