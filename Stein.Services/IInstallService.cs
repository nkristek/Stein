using Stein.Services.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stein.Services
{
    public interface IInstallService
    {
        /// <summary>
        /// List of installed programs read from the registry
        /// </summary>
        IEnumerable<InstalledProgram> InstalledPrograms { get; }

        /// <summary>
        /// Refreshes the list of installed programs
        /// </summary>
        void RefreshInstalledPrograms();

        /// <summary>
        /// Searches in the installed programs if a product code is installed
        /// </summary>
        /// <param name="productCode">Productcode of the program</param>
        /// <returns>If there is a program with the given productcode installed</returns>
        bool IsProductCodeInstalled(string productCode);

        /// <summary>
        /// Installs a installer file
        /// </summary>
        /// <param name="installerPath">Path to the installer file</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be installed without UI</param>
        /// <param name="disableReboot">If automatic reboot should be disabled</param>
        void Install(string installerPath, string logFilePath = null, bool quiet = true, bool disableReboot = true);

        /// <summary>
        /// Installs a installer file asynchronously
        /// </summary>
        /// <param name="installerPath">Path to the installer file</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be installed without UI</param>
        /// <param name="disableReboot">If automatic reboot should be disabled</param>
        /// <returns>Task which installs a installer file</returns>
        Task InstallAsync(string installerPath, string logFilePath = null, bool quiet = true, bool disableReboot = true);

        /// <summary>
        /// Uninstalls and installs a installer file
        /// Warning: reinstalling with a different installer fails, so for now uninstall and install it.
        /// </summary>
        /// <param name="installerPathToInstall">Path to the installer file</param>
        /// <param name="reinstallLogFilePath">Path to a log file for uninstalling (optional)</param>
        /// <param name="quiet">If it should be reinstalled without UI</param>
        /// <param name="disableReboot">If automatic reboot should be disabled</param>
        void Reinstall(string installerPathToReinstall, string reinstallLogFilePath = null, bool quiet = true, bool disableReboot = true);

        /// <summary>
        /// Uninstalls and installs a installer file asynchronously
        /// Warning: reinstalling with a different installer with the same version and productcode fails, use Uninstall+Install.
        /// </summary>
        /// <param name="installerPathToInstall">Path to the installer file</param>
        /// <param name="reinstallLogFilePath">Path to a log file for uninstalling (optional)</param>
        /// <param name="quiet">If it should be reinstalled without UI</param>
        /// <param name="disableReboot">If automatic reboot should be disabled</param>
        /// <returns>Task which uninstalls and installs a installer file</returns>
        Task ReinstallAsync(string installerPathToReinstall, string reinstallLogFilePath = null, bool quiet = true, bool disableReboot = true);

        /// <summary>
        /// Uninstalls a program
        /// </summary>
        /// <param name="productCode">Product code of the installed program</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be uninstalled without UI</param>
        /// <param name="disableReboot">If automatic reboot should be disabled</param>
        void Uninstall(string productCode, string logFilePath = null, bool quiet = true, bool disableReboot = true);

        /// <summary>
        /// Uninstalls a program asynchronously
        /// </summary>
        /// <param name="productCode">Product code of the installed program</param>
        /// <param name="logFilePath">Path to a log file (optional)</param>
        /// <param name="quiet">If it should be uninstalled without UI</param>
        /// <param name="disableReboot">If automatic reboot should be disabled</param>
        /// <returns>Task which uninstalls a installer file</returns>
        Task UninstallAsync(string productCode, string logFilePath = null, bool quiet = true, bool disableReboot = true);
    }
}
