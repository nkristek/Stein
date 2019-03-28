using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stein.ViewModels.Services
{
    public interface IViewModelInstallService
    {
        Task<InstallationResultDialogModel> Install(InstallationViewModel currentInstallation, IReadOnlyList<InstallerViewModel> installerViewModels,
            bool enableSilentInstallation, bool disableReboot, bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs,
            int keepNewestInstallationLogs);

        Task<InstallationResultDialogModel> Uninstall(InstallationViewModel currentInstallation, IReadOnlyList<InstallerViewModel> installerViewModels,
            bool enableSilentInstallation, bool disableReboot, bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs,
            int keepNewestInstallationLogs);

        Task<InstallationResultDialogModel> Custom(InstallationViewModel currentInstallation, IReadOnlyList<InstallerViewModel> installerViewModels,
            bool enableSilentInstallation, bool disableReboot, bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs,
            int keepNewestInstallationLogs);
    }
}
