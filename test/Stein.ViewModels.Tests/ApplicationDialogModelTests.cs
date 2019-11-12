using NKristek.Smaragd.Commands;
using System;
using Xunit;

namespace Stein.ViewModels.Tests
{
    public class ApplicationDialogModelTests
    {
        [Fact]
        public void EntityId_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.EntityId, Guid.NewGuid());
        }

        [Fact]
        public void Name_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.Name, "test");
        }

        [Fact]
        public void EnableSilentInstallation_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.EnableSilentInstallation, true);
        }

        [Fact]
        public void DisableReboot_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.DisableReboot, true);
        }

        [Fact]
        public void EnableInstallationLogging_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.EnableInstallationLogging, true);
        }

        [Fact]
        public void AutomaticallyDeleteInstallationLogs_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.AutomaticallyDeleteInstallationLogs, true);
        }

        [Fact]
        public void KeepNewestInstallationLogsString_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.KeepNewestInstallationLogsString, "test");
        }

        [Fact]
        public void KeepNewestInstallationLogs_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.KeepNewestInstallationLogs, 5);
        }

        [Fact]
        public void FilterDuplicateInstallers_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.FilterDuplicateInstallers, true);
        }

        [Fact]
        public void AvailableProviders_not_null()
        {
            var dialogModel = new ApplicationDialogModel();
            Assert.NotNull(dialogModel.AvailableProviders);
        }

        [Fact]
        public void SelectedProvider_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.SelectedProvider, new DiskInstallerFileBundleProviderViewModel());
        }

        private class TestCommand
            : ViewModelCommand<ApplicationDialogModel>
        {
            protected override void Execute(ApplicationDialogModel viewModel, object parameter)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void OpenLogFolderCommand_property()
        {
            var dialogModel = new ApplicationDialogModel();
            dialogModel.TestProperty(() => dialogModel.OpenLogFolderCommand, new TestCommand(), PropertyTestSettings.IsDirtyIgnored | PropertyTestSettings.IsReadOnlyIgnored);
        }
    }
}
