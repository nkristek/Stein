using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stein.Commands.ApplicationViewModelCommands;
using Stein.Configuration;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace Stein.ViewModels
{
    public class ApplicationViewModel
        : ViewModel
    {
        public ApplicationViewModel(ViewModel parent = null, object view = null) : base(parent, view)
        {
            InstallApplicationCommand = new InstallApplicationCommand(this);
            UninstallApplicationCommand = new UninstallApplicationCommand(this);
        }
        
        public AsyncViewModelCommand<ApplicationViewModel> InstallApplicationCommand { get; private set; }
        
        public AsyncViewModelCommand<ApplicationViewModel> UninstallApplicationCommand { get; private set; }

        public SetupConfiguration AssociatedSetup { get; set; }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (SetProperty(ref _Name, value) && AssociatedSetup != null)
                {
                    AssociatedSetup.Name = Name;
                    AppConfigurationService.SaveConfiguration();
                }
            }
        }

        private string _Path;

        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                if (SetProperty(ref _Path, value) && AssociatedSetup != null)
                {
                    AssociatedSetup.Path = Path;
                    AppConfigurationService.SaveConfiguration();
                }
            }
        }

        private bool _EnableSilentInstallation;

        public bool EnableSilentInstallation
        {
            get
            {
                return _EnableSilentInstallation;
            }

            set
            {
                if (SetProperty(ref _EnableSilentInstallation, value) && AssociatedSetup != null)
                {
                    AssociatedSetup.EnableSilentInstallation = EnableSilentInstallation;
                    AppConfigurationService.SaveConfiguration();
                }
            }
        }

        private ObservableCollection<InstallerBundleViewModel> _InstallerBundles = new ObservableCollection<InstallerBundleViewModel>();

        public ObservableCollection<InstallerBundleViewModel> InstallerBundles
        {
            get
            {
                return _InstallerBundles;
            }

            set
            {
                SetProperty(ref _InstallerBundles, value);
            }
        }

        private InstallerBundleViewModel _SelectedInstallerBundle;

        public InstallerBundleViewModel SelectedInstallerBundle
        {
            get
            {
                return _SelectedInstallerBundle;
            }

            set
            {
                SetProperty(ref _SelectedInstallerBundle, value);
            }
        }
    }
}
