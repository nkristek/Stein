using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TempManager.Configuration;
using TempManager.Services;
using TempManager.ViewModels;
using WpfBase.Commands;

namespace TempManager.Commands.MainViewModelCommands
{
    class AddApplicationCommand
        : ViewModelCommand<MainViewModel>
    {
        public AddApplicationCommand(MainViewModel parent, object view = null) : base(parent, view) { }

        public override void Execute(MainViewModel viewModel, object view, object parameter)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                if (String.IsNullOrWhiteSpace(dialog.SelectedPath) || PathContainsInvalidChars(dialog.SelectedPath))
                {
                    MessageBox.Show("Selected path is not valid!");
                    return;
                }
                
                var setupConfiguration = new SetupConfiguration()
                {
                    Path = dialog.SelectedPath
                };
                AppConfigurationService.CurrentConfiguration.Setups.Add(setupConfiguration);
                if (!AppConfigurationService.SaveConfiguration())
                    return;

                viewModel.Applications.Add(ViewModelService.CreateApplicationViewModel(setupConfiguration, viewModel));
            }
        }

        private bool PathContainsInvalidChars(string path)
        {
            return String.Join(String.Empty, path.Split(Path.GetInvalidPathChars())).Count() != path.Count();
        }
    }
}
