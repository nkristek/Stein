using System;
using nkristek.MVVMBase.ViewModels;
using Stein.Localizations;

namespace Stein.ViewModels
{
    public class InstallationResultViewModel
        : ViewModel
    {
        /// <summary>
        /// The result of the finished installation
        /// </summary>
        [PropertySource(nameof(InstallCount), nameof(ReinstallCount), nameof(UninstallCount), nameof(FailedCount))]
        public string Result
        {
            get
            {
                var resultMessage = String.Format(Strings.DidInstallXPrograms, InstallCount, ReinstallCount, UninstallCount);
                if (FailedCount > 0)
                    resultMessage = String.Join("\n", resultMessage, String.Format(Strings.XInstallersFailed, FailedCount));
                return resultMessage;
            }
        }

        private int _InstallCount;
        /// <summary>
        /// How many installers were installed
        /// </summary>
        public int InstallCount
        {
            get { return _InstallCount; }
            set { SetProperty(ref _InstallCount, value); }
        }

        private int _UninstallCount;
        /// <summary>
        /// How many installers were uninstalled
        /// </summary>
        public int UninstallCount
        {
            get { return _UninstallCount; }
            set { SetProperty(ref _UninstallCount, value); }
        }

        private int _ReinstallCount;
        /// <summary>
        /// How many installers were reinstalled
        /// </summary>
        public int ReinstallCount
        {
            get { return _ReinstallCount; }
            set { SetProperty(ref _ReinstallCount, value); }
        }

        private int _FailedCount;
        /// <summary>
        /// How many installers failed to process
        /// </summary>
        public int FailedCount
        {
            get { return _FailedCount; }
            set { SetProperty(ref _FailedCount, value); }
        }
    }
}
