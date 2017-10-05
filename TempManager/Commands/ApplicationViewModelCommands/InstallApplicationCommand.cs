﻿ using System;
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
        public InstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            if (viewModel == null)
                return false;

            if (viewModel.SelectedInstallerBundle == null)
                return false;

            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            if (mainViewModel == null)
                return false;
            
            return mainViewModel.CurrentInstallation == null && viewModel.InstallerBundles.Any() && !viewModel.InstallerBundles.Any(ib => ib.Installers.Any(i => i.IsInstalled));
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var installers = viewModel.SelectedInstallerBundle.Installers;

            var installerCount = installers.Count();
            var currentInstaller = 0;

            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            mainViewModel.CurrentInstallation = new InstallationViewModel()
            {
                Type = InstallationViewModel.InstallationType.Install
            };

            foreach (var installer in installers)
            {
                mainViewModel.CurrentInstallation.Name = installer.Name;
                mainViewModel.CurrentInstallation.Progress = ((currentInstaller * 100) / (installerCount));
                currentInstaller++;

                Debug.WriteLine("Installing " + installer.Name);
                await InstallService.InstallAsync(installer);
            }

            mainViewModel.CurrentInstallation = null;
        }
    }
}
