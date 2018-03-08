using System;
using nkristek.MVVMBase.ViewModels;
namespace nkristek.Stein.ViewModels
{
    public class FinishedInstallationViewModel
        : ViewModel
    {
        public FinishedInstallationViewModel(ViewModel parent = null, object view = null) : base(parent, view) { }
        
        private string _Result;
        /// <summary>
        /// The result of the finished installation
        /// </summary>
        public string Result
        {
            get
            {
                return _Result;
            }

            set
            {
                SetProperty(ref _Result, value);
            }
        }
    }
}
