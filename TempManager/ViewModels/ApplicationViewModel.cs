using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempManager.Commands.ApplicationViewModelCommands;
using WpfBase.Commands;
using WpfBase.ViewModels;

namespace TempManager.ViewModels
{
    public class ApplicationViewModel
        : ViewModel
    {
        public ApplicationViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }

        public AsyncViewModelCommand<ApplicationViewModel> InstallApplicationCommand
        {
            get
            {
                return new InstallApplicationCommand(this);
            }
        }

        public AsyncViewModelCommand<ApplicationViewModel> UninstallApplicationCommand
        {
            get
            {
                return new UninstallApplicationCommand(this);
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                SetProperty(ref _Name, value);
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
                SetProperty(ref _Path, value);
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
