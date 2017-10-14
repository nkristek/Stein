using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stein.Configuration;
using Stein.Services;
using Stein.ViewModels;
using WpfBase.Commands;
using WpfBase.Extensions;

namespace Stein.Commands.MainWindowViewModelCommands
{
    class AddApplicationCommand
        : AsyncViewModelCommand<MainWindowViewModel>
    {
        public AddApplicationCommand(MainWindowViewModel parent) : base(parent) { }

        public override bool CanExecute(MainWindowViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override async Task ExecuteAsync(MainWindowViewModel viewModel, object view, object parameter)
        {
            string selectedPath;
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                selectedPath = dialog.SelectedPath;
            }

            if (String.IsNullOrWhiteSpace(selectedPath) || selectedPath.ContainsInvalidPathChars())
            {
                MessageBox.Show("Selected path is not valid!");
                return;
            }

            var setupConfiguration = new SetupConfiguration()
            {
                Name = new DirectoryInfo(selectedPath).Name,
                Path = selectedPath,
                EnableSilentInstallation = true
            };

            var application = await Task.Run(() =>
            {
                AppConfigurationService.CurrentConfiguration.Setups.Add(setupConfiguration);
                if (!AppConfigurationService.SaveConfiguration())
                    return null;

                return ViewModelService.CreateApplicationViewModel(setupConfiguration, viewModel);
            });

            if (application != null)
                viewModel.Applications.Add(application);
            else
                await viewModel.RefreshApplicationsCommand.ExecuteAsync(null);
        }
    }
}
