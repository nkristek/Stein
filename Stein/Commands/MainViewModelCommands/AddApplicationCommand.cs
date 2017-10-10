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

namespace Stein.Commands.MainViewModelCommands
{
    class AddApplicationCommand
        : ViewModelCommand<MainViewModel>
    {
        public AddApplicationCommand(MainViewModel parent) : base(parent) { }

        public override bool CanExecute(MainViewModel viewModel, object view, object parameter)
        {
            return viewModel.CurrentInstallation == null;
        }

        public override void Execute(MainViewModel viewModel, object view, object parameter)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                if (String.IsNullOrWhiteSpace(dialog.SelectedPath) || dialog.SelectedPath.ContainsInvalidPathChars())
                {
                    MessageBox.Show("Selected path is not valid!");
                    return;
                }

                var setupConfiguration = new SetupConfiguration()
                {
                    Name = new DirectoryInfo(dialog.SelectedPath).Name,
                    Path = dialog.SelectedPath,
                    EnableSilentInstallation = true
                };
                AppConfigurationService.CurrentConfiguration.Setups.Add(setupConfiguration);
                if (!AppConfigurationService.SaveConfiguration())
                    return;

                viewModel.Applications.Add(ViewModelService.CreateApplicationViewModel(setupConfiguration, viewModel));
            }
        }
    }
}
