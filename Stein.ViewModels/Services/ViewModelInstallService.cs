using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Stein.Helpers;
using Stein.Services.InstallService;
using Stein.Services.InstallService.Arguments;
using Stein.ViewModels.Types;

namespace Stein.ViewModels.Services
{
    public static class ViewModelInstallService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private class DownloadResult
        {
            public IList<KeyValuePair<InstallerViewModel, IDownloadResult>> SucceededDownloads = new List<KeyValuePair<InstallerViewModel, IDownloadResult>>();
            public IList<KeyValuePair<InstallerViewModel, IDownloadResult>> FailedDownloads = new List<KeyValuePair<InstallerViewModel, IDownloadResult>>();
            public IList<KeyValuePair<InstallerViewModel, IDownloadResult>> CancelledDownloads = new List<KeyValuePair<InstallerViewModel, IDownloadResult>>();
        }
        
        public static async Task<InstallationResultDialogModel> Install(IViewModelService viewModelService, IInstallService installService, InstallationViewModel currentInstallationViewModel, IReadOnlyList<InstallerViewModel> installerViewModels, bool enableSilentInstallation, bool disableReboot, bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs, int keepNewestInstallationLogs, bool filterDuplicateInstallers)
        {
            if (viewModelService == null)
                throw new ArgumentNullException(nameof(viewModelService));
            if (installService == null)
                throw new ArgumentNullException(nameof(installService));
            if (currentInstallationViewModel == null)
                throw new ArgumentNullException(nameof(currentInstallationViewModel));
            if (installerViewModels == null)
                throw new ArgumentNullException(nameof(installerViewModels));

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
                filterDuplicateInstallers);
        }
        
        public static async Task<InstallationResultDialogModel> Uninstall(IViewModelService viewModelService, IInstallService installService, InstallationViewModel currentInstallationViewModel, IReadOnlyList<InstallerViewModel> installerViewModels, bool enableSilentInstallation, bool disableReboot,
            bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs, int keepNewestInstallationLogs, bool filterDuplicateInstallers)
        {
            if (viewModelService == null)
                throw new ArgumentNullException(nameof(viewModelService));
            if (installService == null)
                throw new ArgumentNullException(nameof(installService));
            if (currentInstallationViewModel == null)
                throw new ArgumentNullException(nameof(currentInstallationViewModel));
            if (installerViewModels == null)
                throw new ArgumentNullException(nameof(installerViewModels));

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
                filterDuplicateInstallers);
        }
        
        public static async Task<InstallationResultDialogModel> Custom(IViewModelService viewModelService, IInstallService installService, InstallationViewModel currentInstallationViewModel, IReadOnlyList<InstallerViewModel> installerViewModels, bool enableSilentInstallation, bool disableReboot,
            bool enableInstallationLogging, bool automaticallyDeleteInstallationLogs, int keepNewestInstallationLogs, bool filterDuplicateInstallers)
        {
            if (viewModelService == null)
                throw new ArgumentNullException(nameof(viewModelService));
            if (installService == null)
                throw new ArgumentNullException(nameof(installService));
            if (currentInstallationViewModel == null)
                throw new ArgumentNullException(nameof(currentInstallationViewModel));
            if (installerViewModels == null)
                throw new ArgumentNullException(nameof(installerViewModels));

            var installationResultDialogModel = viewModelService.CreateViewModel<InstallationResultDialogModel>(currentInstallationViewModel);
            var cancellationToken = currentInstallationViewModel.CancellationTokenSource.Token;

            using (var tempFileCollection = CreateTempFileCollection())
            {
                var downloadResults = await Download(installerViewModels, tempFileCollection, currentInstallationViewModel);

                foreach (var failedDownload in downloadResults.FailedDownloads)
                {
                    var installerViewModel = failedDownload.Key;
                    var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
                    if (!(failedDownload.Value is FailedDownloadResult downloadResult))
                    {
                        var exception = new Exception($"Unexpected download result type \"{failedDownload.Value.GetType().Name}\", should be \"{nameof(FailedDownloadResult)}\"");
                        Log.Error(exception);
                        var exceptionViewModel = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
                        installationResultViewModel.Exception = exceptionViewModel;
                    }
                    else
                    {
                        installationResultViewModel.Exception = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, downloadResult.Exception);
                    }

                    installationResultViewModel.State = InstallationResultState.DownloadFailed;
                    installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                }

                if (downloadResults.CancelledDownloads.Count > 0)
                {
                    Log.Info("At least one download was cancelled, installation will be skipped.");

                    foreach (var download in downloadResults.CancelledDownloads.Concat(downloadResults.SucceededDownloads))
                    {
                        var installerViewModel = download.Key;
                        var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);
                        installationResultViewModel.State = InstallationResultState.Cancelled;
                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                    }

                    return installationResultDialogModel;
                }

                if (!downloadResults.SucceededDownloads.Any())
                {
                    Log.Info($"No download succeeded (failed: {downloadResults.FailedDownloads.Count}, cancelled: {downloadResults.CancelledDownloads.Count}).");
                    return installationResultDialogModel;
                }

                Log.Info($"Starting operation with {downloadResults.SucceededDownloads.Count} installers.");

                var logFolderPath = enableInstallationLogging ? GetOrCreateLogFileFolderPath(currentInstallationViewModel.Name) : null;
                installationResultDialogModel.LogFolderPath = logFolderPath;

                var installedInstallerViewModels = new List<InstallerViewModel>();
                foreach (var download in downloadResults.SucceededDownloads)
                {
                    currentInstallationViewModel.CurrentInstallerIndex++;
                    var installerViewModel = download.Key;
                    var installationResultViewModel = viewModelService.CreateViewModel<InstallationResultViewModel>(installerViewModel);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Log.Info("The operation was cancelled.");
                        installationResultViewModel.State = InstallationResultState.Cancelled;
                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

                    if (!(download.Value is SucceededDownloadResult downloadResult))
                    {
                        var exception = new Exception($"Unexpected download result type \"{download.Value.GetType().Name}\", should be \"{nameof(SucceededDownloadResult)}\"");
                        Log.Error(exception);
                        var exceptionViewModel = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
                        installationResultViewModel.Exception = exceptionViewModel;
                        installationResultViewModel.State = InstallationResultState.DownloadFailed;
                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

                    if (filterDuplicateInstallers && installedInstallerViewModels.Any(i => i.Name == installerViewModel.Name))
                    {
                        Log.Info($"Installer \"{installerViewModel.FileName}\" with the same application name \"{installerViewModel.Name}\" already processed, file will be skipped.");
                        installationResultViewModel.State = InstallationResultState.Skipped;
                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

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
                                        Log.Info($"The application \"{installerViewModel.Name}\" is already installed. It will now be uninstalled first.");

                                    if (String.IsNullOrWhiteSpace(installerViewModel.ProductCode))
                                    {
                                        var exception = new Exception($"Uninstalling \"{installerViewModel.FileName}\" failed: ProductCode is not set.");
                                        Log.Error(exception);
                                        var exceptionViewModel = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
                                        installationResultViewModel.Exception = exceptionViewModel;
                                        installationResultViewModel.State = InstallationResultState.InstallationFailed;
                                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                                    }
                                    else
                                    {
                                        currentInstallationViewModel.CurrentOperation = InstallationOperation.Uninstall;

                                        var uninstallLogFilePath = GetLogFilePathForInstaller(logFolderPath, installerViewModel.Name, "uninstall");
                                        installationResultViewModel.InstallationLogFilePaths.Add(uninstallLogFilePath);
                                        var uninstallArguments = GetArguments(
                                            enableSilentInstallation,
                                            disableReboot,
                                            enableInstallationLogging,
                                            uninstallLogFilePath).ToArray();
                                        await installService.PerformAsync(new Operation(installerViewModel.ProductCode, OperationType.Uninstall, uninstallArguments));
                                    }
                                }

                                Log.Info($"Installing \"{installerViewModel.FileName}\" now.");

                                currentInstallationViewModel.CurrentOperation = InstallationOperation.Install;

                                var installLogFilePath = GetLogFilePathForInstaller(logFolderPath, installerViewModel.Name, "install");
                                installationResultViewModel.InstallationLogFilePaths.Add(installLogFilePath);
                                var installArguments = GetArguments(
                                    enableSilentInstallation,
                                    disableReboot,
                                    enableInstallationLogging,
                                    installLogFilePath).ToArray();
                                await installService.PerformAsync(new Operation(downloadResult.TempFileName, OperationType.Install, installArguments));

                                installedInstallerViewModels.Add(installerViewModel);
                                break;
                            case InstallerOperation.Uninstall:
                                if (installerViewModel.IsInstalled == false)
                                {
                                    var exception = new InvalidOperationException($"Application \"{installerViewModel.Name}\" is not installed. Uninstall is not possible.");
                                    Log.Warn(exception);
                                    var exceptionViewModel = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
                                    installationResultViewModel.Exception = exceptionViewModel;
                                    installationResultViewModel.State = InstallationResultState.InstallationFailed;
                                    installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                                }
                                else
                                {
                                    if (installerViewModel.IsInstalled == null)
                                        Log.Warn($"Installation status of file \"{installerViewModel.FileName}\" is unavailable, trying to uninstall.");
                                    else
                                        Log.Info($"The application \"{installerViewModel.Name}\" will now be uninstalled.");

                                    if (String.IsNullOrWhiteSpace(installerViewModel.ProductCode))
                                    {
                                        var exception = new Exception($"Uninstalling \"{installerViewModel.FileName}\" failed: ProductCode is not set.");
                                        Log.Error(exception);
                                        var exceptionViewModel = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
                                        installationResultViewModel.Exception = exceptionViewModel;
                                        installationResultViewModel.State = InstallationResultState.InstallationFailed;
                                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                                    }
                                    else
                                    {
                                        currentInstallationViewModel.CurrentOperation = InstallationOperation.Uninstall;

                                        var uninstallLogFilePath = GetLogFilePathForInstaller(logFolderPath, installerViewModel.Name, "uninstall");
                                        installationResultViewModel.InstallationLogFilePaths.Add(uninstallLogFilePath);
                                        var uninstallArguments = GetArguments(
                                            enableSilentInstallation,
                                            disableReboot,
                                            enableInstallationLogging,
                                            uninstallLogFilePath).ToArray();
                                        await installService.PerformAsync(new Operation(installerViewModel.ProductCode, OperationType.Uninstall, uninstallArguments));
                                    }
                                }
                                installedInstallerViewModels.Add(installerViewModel);
                                break;
                            default:
                                installationResultViewModel.State = InstallationResultState.Skipped;
                                installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                                currentInstallationViewModel.ProcessedInstallerFileCount++;
                                continue;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                        var exceptionViewModel = viewModelService.CreateViewModel<ExceptionViewModel>(installationResultViewModel, exception);
                        installationResultViewModel.Exception = exceptionViewModel;
                        installationResultViewModel.State = InstallationResultState.InstallationFailed;
                        installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                        currentInstallationViewModel.ProcessedInstallerFileCount++;
                        continue;
                    }

                    installationResultViewModel.State = InstallationResultState.Success;
                    installationResultDialogModel.InstallationResults.Add(installationResultViewModel);
                    currentInstallationViewModel.ProcessedInstallerFileCount++;
                }

                currentInstallationViewModel.CurrentOperation = InstallationOperation.None;

                if (enableInstallationLogging && automaticallyDeleteInstallationLogs)
                {
                    try
                    {
                        RemoveOldestFiles(logFolderPath, keepNewestInstallationLogs);
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

        private static ITempFileCollection CreateTempFileCollection()
        {
            return new TempFileCollection(GetDownloadPath());
        }
        
        private static string GetDownloadPath()
        {
            var downloadFolderPath = Path.Combine(Path.GetTempPath(), Assembly.GetEntryAssembly().GetName().Name, "Downloads");
            if (!Directory.Exists(downloadFolderPath))
                Directory.CreateDirectory(downloadFolderPath);
            return downloadFolderPath;
        }

        private static async Task<DownloadResult> Download(IReadOnlyCollection<InstallerViewModel> installerViewModels, ITempFileCollection tempFileCollection, InstallationViewModel currentInstallation)
        {
            Log.Info($"Starting download of {installerViewModels.Count} files.");

            var progress = new Progress<double>(input => currentInstallation.DownloadProgress = input);
            var downloadResults = (await Download(installerViewModels, tempFileCollection, progress, 3, currentInstallation.CancellationTokenSource.Token)).ToList();

            var succeededDownloads = downloadResults.Where(dr => dr.Value.Result == DownloadResultState.CompletedSuccessfully).ToList();
            var failedDownloads = downloadResults.Where(dr => dr.Value.Result == DownloadResultState.Failed).ToList();
            var cancelledDownloads = downloadResults.Where(dr => dr.Value.Result == DownloadResultState.Cancelled).ToList();

            Log.Info($"Download finished. {succeededDownloads.Count} succeeded, {failedDownloads.Count} failed and {cancelledDownloads.Count} cancelled.");

            return new DownloadResult
            {
                SucceededDownloads = succeededDownloads,
                FailedDownloads = failedDownloads,
                CancelledDownloads = cancelledDownloads
            };
        }

        private static async Task<IEnumerable<KeyValuePair<InstallerViewModel, IDownloadResult>>> Download(IReadOnlyCollection<InstallerViewModel> installerViewModels, ITempFileCollection tempFileCollection, IProgress<double> progress, int parallelDownloads = 3, CancellationToken cancellationToken = default)
        {
            using (var semaphore = new SemaphoreSlim(parallelDownloads))
            {
                var downloadTasks = new List<KeyValuePair<InstallerViewModel, Task<IDownloadResult>>>();
                var progressValues = new ConcurrentDictionary<InstallerViewModel, double>();
                var results = new List<KeyValuePair<InstallerViewModel, IDownloadResult>>();

                foreach (var installer in installerViewModels)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        results.Add(new KeyValuePair<InstallerViewModel, IDownloadResult>(installer, new CancelledDownloadResult()));
                        continue;
                    }
                    
                    await semaphore.WaitAsync(cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        results.Add(new KeyValuePair<InstallerViewModel, IDownloadResult>(installer, new CancelledDownloadResult()));
                        semaphore.Release();
                        continue;
                    }

                    var installerFileProvider = installer.InstallerFileProvider;
                    if (installerFileProvider == null)
                        continue;
                    
                    var filePath = tempFileCollection.CreateUniqueFileName("msi");
                    var individualProgress = progress != null ? new Progress<double>(input =>
                    {
                        progressValues[installer] = input;
                        progress.Report(progressValues.Values.Sum() / installerViewModels.Count);
                    }) : null;

                    downloadTasks.Add(new KeyValuePair<InstallerViewModel, Task<IDownloadResult>>(installer, Task.Run(async () =>
                    {
                        try
                        {
                            return await Download(installerFileProvider, filePath, individualProgress, cancellationToken);
                        }
                        finally
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            // note: semaphore will only be disposed after all tasks are finished since every task is awaited
                            semaphore.Release();
                        }
                    })));
                }

                foreach (var task in downloadTasks)
                    results.Add(new KeyValuePair<InstallerViewModel, IDownloadResult>(task.Key, await task.Value));
                return results;
            }
        }

        private static async Task<IDownloadResult> Download(IInstallerFileProvider installerFileProvider, string filePath, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (installerFileProvider == null)
                    throw new ArgumentNullException(nameof(installerFileProvider));
                if (String.IsNullOrWhiteSpace(filePath))
                    throw new ArgumentNullException(nameof(filePath));

                cancellationToken.ThrowIfCancellationRequested();
                await installerFileProvider.SaveFileAsync(filePath, progress, cancellationToken);
                return new SucceededDownloadResult(filePath);
            }
            catch (OperationCanceledException)
            {
                return new CancelledDownloadResult();
            }
            catch (Exception exception)
            {
                return new FailedDownloadResult(exception);
            }
        }

        private static void RemoveOldestFiles(string folderPath, int keepNewestLogFiles)
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
            if (File.Exists(logFilePath))
                throw new Exception("File already exists");
            return logFilePath;
        }
    }
}
