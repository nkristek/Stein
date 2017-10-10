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
    public class UninstallApplicationCommand
        : AsyncViewModelCommand<ApplicationViewModel>
    {
        public UninstallApplicationCommand(ApplicationViewModel parent) : base(parent) { }

        public override bool CanExecute(ApplicationViewModel viewModel, object view, object parameter)
        {
            if (viewModel.SelectedInstallerBundle == null)
                return false;

            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            return mainViewModel != null && mainViewModel.CurrentInstallation == null && viewModel.InstallerBundles.Any() && viewModel.InstallerBundles.Any(ib => ib.Installers.Any(i => i.IsInstalled));
        }

        public override async Task ExecuteAsync(ApplicationViewModel viewModel, object view, object parameter)
        {
            var installers = viewModel.SelectedInstallerBundle.Installers.Where(i => i.IsInstalled);
            
            var mainViewModel = viewModel.FirstParentOfType<MainViewModel>();
            mainViewModel.CurrentInstallation = new InstallationViewModel(mainViewModel)
            {
                Type = InstallationViewModel.InstallationType.Uninstall,
                InstallerCount = installers.Count(),
                CurrentIndex = 0
            };

            const string caption = "Confirm uninstall";
            var questionBuilder = new StringBuilder();
            questionBuilder.AppendLine("Do you want to uninstall the following installers:");
            foreach (var installer in installers)
                questionBuilder.AppendLine(installer.Name);

            if (MessageBox.Show(questionBuilder.ToString(), caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var currentInstaller = 1;
                foreach (var installer in installers)
                {
                    mainViewModel.CurrentInstallation.Name = installer.Name;
                    mainViewModel.CurrentInstallation.CurrentIndex = currentInstaller;
                    
                    Debug.WriteLine("Uninstalling " + installer.Name);
                    await InstallService.UninstallAsync(installer, viewModel.EnableSilentInstallation);
                    currentInstaller++;
                }
            }
            
            mainViewModel.CurrentInstallation = null;
            mainViewModel.RefreshApplicationsCommand.Execute(null);
        }
    }
}
