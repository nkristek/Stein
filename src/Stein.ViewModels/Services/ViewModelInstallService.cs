using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Stein.Localization;
using Stein.Presentation;
using Stein.Services.InstallService;
using Stein.Services.InstallService.Arguments;
using Stein.Utility;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Services
{
    public static class ViewModelInstallService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static async Task<InstallationResultDialogModel> Install(IViewModelService viewModelService, IInstallService installService, InstallationViewModel currentInstallationViewModel, IReadOnlyList<InstallerViewModel> installerViewModels, bool enableSilentInstallation, bool disableReboot, 
            bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs, int keepNewestInstallationLogs, bool filterDuplicateInstallers, string downloadFolderPath)
        {
            if (viewModelService == null)
                throw new ArgumentNullException(nameof(viewModelService));
            if (installService == null)
                throw new ArgumentNullException(nameof(installService));
            if (currentInstallationViewModel == null)
                throw new ArgumentNullException(nameof(currentInstallationViewModel));
            if (installerViewModels == null)
                throw new ArgumentNullException(nameof(installerViewModels));
            if (String.IsNullOrEmpty(downloadFolderPath))
                throw new ArgumentNullException(nameof(downloadFolderPath));

            foreach (var installer in installerViewModels)
                installer.SelectedOperation = InstallerOperation.Install;

            return await Custom(
                viewModelService, 
                installService, 
                currentInstallationViewModel, 
                installerViewModels, 
                enableSilentInstallation, 
                disableReboot,
                enableInstallationLogging, 
                automaticallyDeleteInstallationLogs, 
                keepNewestInstallationLogs,
                filterDuplicateInstallers,
                downloadFolderPath);
        }
        
        public static async Task<InstallationResultDialogModel> Uninstall(IViewModelService viewModelService, IInstallService installService, InstallationViewModel currentInstallationViewModel, IReadOnlyList<InstallerViewModel> installerViewModels, bool enableSilentInstallation, bool disableReboot,
            bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs, int keepNewestInstallationLogs, bool filterDuplicateInstallers, string downloadFolderPath)
        {
            if (viewModelService == null)
                throw new ArgumentNullException(nameof(viewModelService));
            if (installService == null)
                throw new ArgumentNullException(nameof(installService));
            if (currentInstallationViewModel == null)
                throw new ArgumentNullException(nameof(currentInstallationViewModel));
            if (installerViewModels == null)
                throw new ArgumentNullException(nameof(installerViewModels));
            if (String.IsNullOrEmpty(downloadFolderPath))
                throw new ArgumentNullException(nameof(downloadFolderPath));

            foreach (var installer in installerViewModels)
                installer.SelectedOperation = InstallerOperation.Uninstall;

            return await Custom(
                viewModelService,
                installService,
                currentInstallationViewModel,
                installerViewModels,
                enableSilentInstallation,
                disableReboot,
                enableInstallationLogging,
                automaticallyDeleteInstallationLogs,
                keepNewestInstallationLogs,
                filterDuplicateInstallers,
                downloadFolderPath);
        }
        
        public static async Task<InstallationResultDialogModel> Custom(IViewModelService viewModelService, IInstallService installService, InstallationViewModel currentInstallationViewModel, IReadOnlyList<InstallerViewModel> installerViewModels, bool enableSilentInstallation, bool disableReboot,
            bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs, int keepNewestInstallationLogs, bool filterDuplicateInstallers, string downloadFolderPath)
        {
            if (viewModelService == null)
                throw new ArgumentNullException(nameof(viewModelService));
            if (installService == null)
                throw new ArgumentNullException(nameof(installService));
            if (currentInstallationViewModel == null)
                throw new ArgumentNullException(nameof(currentInstallationViewModel));
            if (installerViewModels == null)
                throw new ArgumentNullException(nameof(installerViewModels));
            if (String.IsNullOrEmpty(downloadFolderPath))
                throw new ArgumentNullException(nameof(downloadFolderPath));

            var cancellationToken = currentInstallationViewModel.CancellationTokenSource.Token;
            var installationResultDialogModel = viewModelService.CreateViewModel<InstallationResultDialogModel>(currentInstallationViewModel);

            var logFolderPath = enableInstallationLogging ? GetOrCreateLogFileFolderPath(currentInstallationViewModel.Name) : null;
            installationResultDialogModel.LogFolderPath = logFolderPath;

            var sessionDownloadPath = Path.Combine(downloadFolderPath, currentInstallationViewModel.Name);
            if (!Directory.Exists(sessionDownloadPath))
                Directory.CreateDirectory(sessionDownloadPath);
            using (var tempFileCollection = new TempFileCollection(sessionDownloadPath))
            {
                Log.Info($"Starting download of {installerViewModels.Count} files.");
                var downloadResults = await Download(installerViewModels, currentInstallationViewModel, tempFileCollection, 3, cancellationToken);
                Log.Info($"Download finished.");

                if (downloadResults.Any(dr => dr.Value.Result == DownloadResultState.Cancelled))
                {
                    Log.Info("At least one download was cancelled, installation will be skipped.");

                    foreach (var download in downloadResults)
                    {
                        currentInstallationViewModel.CurrentInstallerIndex++;
                        var installerViewModel = download.Key;
                        var downloadResult = download.Value;

                        var installationResultViewModel = downloadResult.Result == DownloadResultState.Failed 
                            ? CreateDownloadFailedResult(viewModelService, installerViewModel, downloadResult) 
                            : CreateCancelledResult(viewModelService, installerViewModel);

                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                    }

                    return installationResultDialogModel;
                }
                
                if (downloadResults.All(dr => dr.Value.Result != DownloadResultState.CompletedSuccessfully))
                {
                    var failedCount = downloadResults.Count(dr => dr.Value.Result == DownloadResultState.Failed);
                    Log.Warn($"No download succeeded (failed downloads: {failedCount}).");
                }
                else
                {
                    var succeededCount = downloadResults.Count(dr => dr.Value.Result == DownloadResultState.CompletedSuccessfully);
                    currentInstallationViewModel.TotalInstallerFileCount = succeededCount;
                    Log.Info($"Starting operation with {succeededCount} installers.");
                }

                var installedInstallerViewModels = new List<InstallerViewModel>();
                foreach (var download in downloadResults)
                {
                    currentInstallationViewModel.CurrentInstallerIndex++;
                    var installerViewModel = download.Key;
                    var downloadResult = download.Value;

                    if (downloadResult.Result == DownloadResultState.Failed)
                    {
                        var failedResultViewModel = CreateDownloadFailedResult(viewModelService, installerViewModel, downloadResult);
                        installationResultDialogModel.InstallationResults.Add(failedResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }
                    
                    if (cancellationToken.IsCancellationRequested)
                    {
                        var cancelledResultViewModel = CreateCancelledResult(viewModelService, installerViewModel);
                        installationResultDialogModel.InstallationResults.Add(cancelledResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

                    if (!(downloadResult is SucceededDownloadResult succeededDownloadResult))
                    {
                        var exception = new Exception($"Unexpected download result type \"{downloadResult.GetType().Name}\", should be \"{nameof(SucceededDownloadResult)}\"");
                        Log.Error(exception);
                        var failedResultViewModel = CreateDownloadFailedResult(viewModelService, installerViewModel, exception);
                        installationResultDialogModel.InstallationResults.Add(failedResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

                    if (filterDuplicateInstallers && installedInstallerViewModels.Any(i => i.Name == installerViewModel.Name))
                    {
                        Log.Info($"Installer \"{installerViewModel.FileName}\" with the same application name \"{installerViewModel.Name}\" already processed, file will be skipped.");
                        var skippedResultViewModel = CreateSkippedResult(viewModelService, installerViewModel);
                        installationResultDialogModel.InstallationResults.Add(skippedResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

                    var installationResultViewModel = await PerformOperation(
                        viewModelService, 
                        installService, 
                        installerViewModel, 
                        currentInstallationViewModel, 
                        succeededDownloadResult.TempFileName, 
                        enableSilentInstallation, 
                        disableReboot, 
                        enableInstallationLogging, 
                        logFolderPath);
                    installedInstallerViewModels.Add(installerViewModel);
                    installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                    currentInstallationViewModel.ProcessedInstallerFileCount++;
                }

                currentInstallationViewModel.CurrentOperation = InstallationOperation.None;

                if (enableInstallationLogging && automaticallyDeleteInstallationLogs)
                {
                    try
                    {
                        RemoveOldestLogFiles(logFolderPath, keepNewestInstallationLogs);
                    }
                    catch (Exception exception)
                    {
                        Log.Warn("Deleting old log files failed", exception);
                    }
                }
            }
            
            installationResultDialogModel.IsReadOnly = true;
            return installationResultDialogModel;
        }

        private static async Task<InstallationResultViewModel> PerformOperation(IViewModelService viewModelService, IInstallService installService, InstallerViewModel installerViewModel, InstallationViewModel currentInstallationViewModel, string installerFilePath, bool enableSilentInstallation, bool disableReboot,
        bool enableInstallationLogging, string logFolderPath = null)
        {
            var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
            var installLogFilePath = GetLogFilePathForInstaller(logFolderPath, installerViewModel.Name, "install");
            var installArguments = GetArguments(
                enableSilentInstallation,
                disableReboot,
                enableInstallationLogging && !String.IsNullOrEmpty(installLogFilePath),
                installLogFilePath).ToArray();
            var uninstallLogFilePath = GetLogFilePathForInstaller(logFolderPath, installerViewModel.Name, "uninstall");
            var uninstallArguments = GetArguments(
                enableSilentInstallation,
                disableReboot,
                enableInstallationLogging && !String.IsNullOrEmpty(uninstallLogFilePath),
                uninstallLogFilePath).ToArray();
            try
            {
                switch (installerViewModel.SelectedOperation)
                {
                    case InstallerOperation.Install:
                        if (installerViewModel.IsInstalled != false)
                        {
                            if (installerViewModel.IsInstalled == null)
                                Log.Warn($"Installation status of file \"{installerViewModel.FileName}\" is unavailable, trying to uninstall.");
                            else
                                Log.Info($"The application \"{installerViewModel.Name}\" with ProductCode \"{installerViewModel.ProductCode}\" is already installed. It will now be uninstalled first.");

                            if (String.IsNullOrWhiteSpace(installerViewModel.ProductCode))
                            {
                                var exception = new Exception($"Uninstalling \"{installerViewModel.FileName}\" failed: ProductCode is not set.");
                                Log.Error(exception);
                                return CreateInstallationFailedResult(viewModelService, installerViewModel, exception);
                            }

                            Log.Info($"Uninstalling application of installer \"{installerViewModel.FileName}\" now.");
                            currentInstallationViewModel.CurrentOperation = InstallationOperation.Uninstall;
                            
                            installationResultViewModel.InstallationLogFilePaths.Add(uninstallLogFilePath);
                            try
                            {
                                await installService.PerformAsync(new Operation(installerViewModel.ProductCode, OperationType.Uninstall, uninstallArguments));
                                Log.Info($"Finished uninstalling.");
                            }
                            catch (Exception exception)
                            {
                                // suppress exception if installation status is unknown
                                if (installerViewModel.IsInstalled == true)
                                    throw;

                                Log.Warn("Uninstalling failed, but since it wasn't known, if the application was already installed, the operation will continue", exception);
                            }
                        }

                        Log.Info($"Installing \"{installerViewModel.FileName}\" now.");
                        currentInstallationViewModel.CurrentOperation = InstallationOperation.Install;
                        installationResultViewModel.InstallationLogFilePaths.Add(installLogFilePath);
                        await installService.PerformAsync(new Operation(installerFilePath, OperationType.Install, installArguments));
                        Log.Info($"Finished installing.");
                        installationResultViewModel.State = InstallationResultState.Success;
                        break;
                    case InstallerOperation.Uninstall:
                        if (installerViewModel.IsInstalled == null)
                            Log.Warn($"Installation status of file \"{installerViewModel.FileName}\" is unavailable, trying to uninstall.");
                        else if (installerViewModel.IsInstalled == false)
                            Log.Warn($"The application \"{installerViewModel.Name}\" with ProductCode \"{installerViewModel.ProductCode}\" is not installed. Trying to uninstall anyways but it will likely fail.");
                        else
                            Log.Info($"The application \"{installerViewModel.Name}\" with ProductCode \"{installerViewModel.ProductCode}\" will now be uninstalled.");

                        if (String.IsNullOrWhiteSpace(installerViewModel.ProductCode))
                        {
                            var exception = new Exception($"Uninstalling \"{installerViewModel.FileName}\" failed: ProductCode is not set.");
                            Log.Error(exception);
                            return CreateInstallationFailedResult(viewModelService, installerViewModel, exception);
                        }

                        Log.Info($"Uninstalling application of installer \"{installerViewModel.FileName}\" now.");
                        currentInstallationViewModel.CurrentOperation = InstallationOperation.Uninstall;
                        installationResultViewModel.InstallationLogFilePaths.Add(uninstallLogFilePath);
                        await installService.PerformAsync(new Operation(installerViewModel.ProductCode, OperationType.Uninstall, uninstallArguments));
                        Log.Info($"Finished uninstalling.");
                        installationResultViewModel.State = InstallationResultState.Success;
                        break;
                    default:
                        installationResultViewModel.State = InstallationResultState.Skipped;
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return CreateInstallationFailedResult(viewModelService, installerViewModel, exception);
            }

            return installationResultViewModel;
        }

        private static InstallationResultViewModel CreateDownloadFailedResult(IViewModelService viewModelService, InstallerViewModel installerViewModel, IDownloadResult downloadResult)
        {
            if (downloadResult.Result != DownloadResultState.Failed)
                throw new ArgumentException($"Result is {downloadResult.Result.ToString()}, expected {nameof(DownloadResultState.Failed)}.", nameof(downloadResult));

            Exception exception;
            if (downloadResult is FailedDownloadResult failedDownloadResult)
            {
                exception = failedDownloadResult.Exception;
            }
            else
            {
                exception = new Exception(Strings.ErrorOccured);
                Log.Error($"DownloadResult with failed state is not of type {nameof(FailedDownloadResult)}.");
            }

            return CreateDownloadFailedResult(viewModelService, installerViewModel, exception);
        }

        private static InstallationResultViewModel CreateDownloadFailedResult(IViewModelService viewModelService, InstallerViewModel installerViewModel,
            Exception exception)
        {
            var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
            installationResultViewModel.State = InstallationResultState.DownloadFailed;
            installationResultViewModel.Exception = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
            return installationResultViewModel;
        }

        private static InstallationResultViewModel CreateInstallationFailedResult(IViewModelService viewModelService, InstallerViewModel installerViewModel,
            Exception exception)
        {
            var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
            installationResultViewModel.State = InstallationResultState.InstallationFailed;
            installationResultViewModel.Exception = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
            return installationResultViewModel;
        }

        private static InstallationResultViewModel CreateCancelledResult(IViewModelService viewModelService, InstallerViewModel installerViewModel)
        {
            var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
            installationResultViewModel.State = InstallationResultState.Cancelled;
            return installationResultViewModel;
        }

        private static InstallationResultViewModel CreateSkippedResult(IViewModelService viewModelService, InstallerViewModel installerViewModel)
        {
            var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
            installationResultViewModel.State = InstallationResultState.Skipped;
            return installationResultViewModel;
        }

        private static async Task<IList<KeyValuePair<InstallerViewModel, IDownloadResult>>> Download(IReadOnlyCollection<InstallerViewModel> installerViewModels, InstallationViewModel currentInstallation, ITempFileCollection tempFileCollection, int parallelDownloads = 3, CancellationToken cancellationToken = default)
        {
            using (var semaphore = new SemaphoreSlim(parallelDownloads))
            {
                var downloadTasks = new List<KeyValuePair<InstallerViewModel, Task<IDownloadResult>>>();
                var progressValues = new ConcurrentDictionary<InstallerViewModel, double>();

                foreach (var installer in installerViewModels)
                {
                    var installerFileProvider = installer.InstallerFileProvider;
                    if (installerFileProvider == null)
                        continue;

                    var filePath = tempFileCollection.CreateUniqueFileName("msi");
                    var individualProgress = new Progress<double>(input =>
                    {
                        progressValues[installer] = input;
                        currentInstallation.DownloadProgress = progressValues.Values.Sum() / installerViewModels.Count;
                    });
                    var downloadTask = Download(installerFileProvider, filePath, semaphore, individualProgress, cancellationToken);
                    downloadTasks.Add(new KeyValuePair<InstallerViewModel, Task<IDownloadResult>>(installer, downloadTask));
                }

                var downloadResults = new List<KeyValuePair<InstallerViewModel, IDownloadResult>>();
                foreach (var task in downloadTasks)
                    downloadResults.Add(new KeyValuePair<InstallerViewModel, IDownloadResult>(task.Key, await task.Value));
                
                return downloadResults;
            }
        }

        private static async Task<IDownloadResult> Download(IInstallerFileProvider installerFileProvider, string filePath, SemaphoreSlim semaphore, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (installerFileProvider == null)
                    throw new ArgumentNullException(nameof(installerFileProvider));
                if (String.IsNullOrEmpty(filePath))
                    throw new ArgumentNullException(nameof(filePath));

                cancellationToken.ThrowIfCancellationRequested();
                await semaphore.WaitAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
                await installerFileProvider.SaveFileAsync(filePath, progress, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
                return new SucceededDownloadResult(filePath);
            }
            catch (OperationCanceledException)
            {
                return new CancelledDownloadResult();
            }
            catch (Exception exception)
            {
                Log.Error("Download failed", exception);
                return new FailedDownloadResult(exception);
            }
            finally
            {
                try
                {
                    semaphore.Release();
                }
                catch
                {
                }
            }
        }

        private static void RemoveOldestLogFiles(string folderPath, int keepNewestLogFiles)
        {
            foreach (var file in new DirectoryInfo(folderPath).EnumerateFiles().OrderByDescending(f => f.CreationTime).Skip(Math.Max(0, keepNewestLogFiles)))
            {
                try
                {
                    file.Delete();
                }
                catch (Exception exception)
                {
                    Log.Warn("Deleting log file failed", exception);
                }
            }
        }

        private static string GetOrCreateLogFileFolderPath(string name)
        {
            var logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Stein", "Logs", name);
            if (!Directory.Exists(logFolderPath))
                Directory.CreateDirectory(logFolderPath);
            return logFolderPath;
        }

        private static IEnumerable<IOperationArgument> GetArguments(bool enableSilentInstallation, bool disableReboot, bool enableInstallationLogging, string logFilePath = null)
        {
            if (enableSilentInstallation)
                yield return new QuietArgument();

            if (disableReboot)
                yield return new DisableRebootArgument();

            if (enableInstallationLogging)
                yield return new LogFileArgument(logFilePath);
        }

        private static string GetLogFilePathForInstaller(string logFolderName, string installerName, string installMethod)
        {
            var currentDate = DateTime.Now;
            var logFileName = $"{currentDate.Year}-{currentDate.Month}-{currentDate.Day}_{currentDate.Hour}-{currentDate.Minute}-{currentDate.Second}_{installerName}_{installMethod}.txt";
            var logFilePath = Path.Combine(logFolderName, logFileName);
            if (!File.Exists(logFilePath))
                return logFilePath;
            Log.Warn("Error while building file path for the log file: File already exists");
            return null;
        }
    }
}
