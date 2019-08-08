using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NKristek.Smaragd.Attributes;
using NKristek.Smaragd.Commands;
using Stein.Common.InstallService;
using Stein.Common.IOService;
using Stein.Utility;

namespace Stein.ViewModels.Commands.UpdateDialogModelCommands
{
    public class InstallUpdateCommand
        : AsyncViewModelCommand<UpdateDialogModel>
    {
        private readonly IInstallService _installService;

        private readonly IIOService _ioService;

        private readonly string _downloadFolderPath;

        public InstallUpdateCommand(IInstallService installService, IIOService ioService, string downloadFolderPath)
        {
            _installService = installService ?? throw new ArgumentNullException(nameof(installService));
            _ioService = ioService ?? throw new ArgumentNullException(nameof(ioService));
            _downloadFolderPath = downloadFolderPath ?? throw new ArgumentNullException(nameof(downloadFolderPath));
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private CancellationTokenSource CancellationTokenSource
        {
            get => _cancellationTokenSource;
            set
            {
                if (_cancellationTokenSource == value)
                    return;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = value;
                RaiseCanExecuteChanged();
            }
        }

        public bool IsCancelled => CancellationTokenSource.IsCancellationRequested;

        public void Cancel()
        {
            if (IsCancelled)
                return;

            CancellationTokenSource.Cancel();
            RaisePropertyChanged(nameof(IsCancelled));
            RaiseCanExecuteChanged();
            if (Parent != null)
                Parent.IsUpdateCancelled = true;
        }
        
        private double _downloadProgress;

        public double DownloadProgress
        {
            get => _downloadProgress;
            set => SetProperty(ref _downloadProgress, value, out _);
        }

        private void UpdateProgress()
        {
            if (BytesTotal == 0)
                DownloadProgress = 0;
            else
                DownloadProgress = (double)BytesDownloaded / BytesTotal;
        }
        
        private long _bytesDownloaded;

        public long BytesDownloaded
        {
            get => _bytesDownloaded;
            set
            {
                if (SetProperty(ref _bytesDownloaded, value, out _))
                    UpdateProgress();
            }
        }

        private long _bytesTotal;

        public long BytesTotal
        {
            get => _bytesTotal;
            set
            {
                if (SetProperty(ref _bytesTotal, value, out _))
                    UpdateProgress();
            }
        }
        
        /// <inheritdoc />
        [CanExecuteSource(nameof(UpdateDialogModel.UpdateAssets))]
        protected override bool CanExecute(UpdateDialogModel viewModel, object parameter)
        {
            return !IsCancelled && viewModel.UpdateAssets.Any(ua => ua.FileName != null && ua.FileName.Contains(GetCurrentPlatform()) && ua.FileName.EndsWith(".msi"));
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(UpdateDialogModel viewModel, object parameter)
        {
            string installerFilePath = null;
            try
            {
                viewModel.IsUpdateDownloading = true;

                var updateAssetViewModel = viewModel.UpdateAssets.FirstOrDefault(ua =>
                    ua.FileName != null 
                    && ua.FileName.Contains(GetCurrentPlatform()) 
                    && ua.FileName.EndsWith(".msi"));
                if (updateAssetViewModel == null)
                    return;

                installerFilePath = CreateInstallerFilePath(updateAssetViewModel, viewModel.ReleaseTag);
                if (!_ioService.FileExists(installerFilePath))
                    await DownloadInstallerFile(updateAssetViewModel.DownloadUri, installerFilePath);
                StartInstall(installerFilePath);
                Environment.Exit(0);
            }
            catch (OperationCanceledException)
            {
                if (installerFilePath != null)
                    _ioService.DeleteFile(installerFilePath);
            }
            finally
            {
                CancellationTokenSource = new CancellationTokenSource();
                BytesTotal = 0;
                BytesDownloaded = 0;
                viewModel.IsUpdateDownloading = false;
                viewModel.IsUpdateCancelled = false;
                RaiseCanExecuteChanged();
            }
        }

        private string CreateInstallerFilePath(UpdateAssetViewModel updateAssetViewModel, string releaseName)
        {
            var downloadFolderPath = _ioService.PathCombine(_downloadFolderPath, releaseName);
            if (!_ioService.DirectoryExists(downloadFolderPath))
                _ioService.CreateDirectory(downloadFolderPath);
            return _ioService.PathCombine(downloadFolderPath, updateAssetViewModel.FileName);
        }

        private async Task DownloadInstallerFile(Uri downloadUri, string destinationFilePath)
        {
            var progressReporter = new Progress<DownloadProgress>(progress =>
            {
                BytesDownloaded = progress.BytesDownloaded;
                BytesTotal = progress.BytesTotal;
            });
            var httpClient = new HttpClient
            {
                DefaultRequestHeaders = { { "User-Agent", "nkristek/Stein" } }
            };
            using (httpClient)
                await httpClient.DownloadAsync(downloadUri, destinationFilePath, progressReporter, CancellationTokenSource.Token);
        }

        private void StartInstall(string installerFilePath)
        {
#pragma warning disable 4014
            // do not await here
            _installService.PerformAsync(new Operation(installerFilePath, OperationType.Install));
#pragma warning restore 4014
        }

        private static string GetCurrentPlatform()
        {
            if (Environment.Is64BitProcess)
                return "x64";
            return "x86";
        }
    }
}
