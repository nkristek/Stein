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
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public InstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            if (mainWindowViewModel == null || mainWindowViewModel.CurrentInstallation != null)
                return false;
            
            return viewModel.SelectedInstallerBundle != null;
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var mainWindowViewModel = viewModel.Parent as MainWindowViewModel;
            mainWindowViewModel.CurrentInstallation = new InstallationViewModel(mainWindowViewModel)
            {
                Type = InstallationViewModel.InstallationType.Install,
                InstallerCount = 0,
                CurrentIndex = 0
            };

            if (DialogService.ShowDialog(viewModel.SelectedInstallerBundle, "Select installers") == true)
            {
                var installersToInstall = viewModel.SelectedInstallerBundle.Installers.Where(i => i.IsEnabled && !i.IsDisabled);
                mainWindowViewModel.CurrentInstallation.InstallerCount = installersToInstall.Count();
                
                foreach (var installer in installersToInstall)
                {
                    mainWindowViewModel.CurrentInstallation.Name = installer.Name;
                    mainWindowViewModel.CurrentInstallation.CurrentIndex++;

                    if (installer.IsInstalled.HasValue && installer.IsInstalled.Value)
                        await InstallService.ReinstallAsync(installer, viewModel.EnableSilentInstallation);
                    else
                        await InstallService.InstallAsync(installer, viewModel.EnableSilentInstallation);
                }
            }

            foreach (var installer in viewModel.SelectedInstallerBundle.Installers)
                installer.IsDisabled = false;

            mainWindowViewModel.CurrentInstallation = null;
            mainWindowViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
