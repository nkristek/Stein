using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBase.ViewModels;

namespace TempManager.ViewModels
{
    public class InstallationViewModel
        : ViewModel
    {
        public InstallationViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }

        public enum InstallationType
        {
            Install,

            Uninstall
        }

        private InstallationType _Type;

        public InstallationType Type
        {
            get
            {
                return _Type;
            }

            set
            {
                SetProperty(ref _Type, value);
            }
        }

        private double _Progress;

        public double Progress
        {
            get
            {
                return _Progress;
            }

            set
            {
                SetProperty(ref _Progress, value);
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
    }
}
