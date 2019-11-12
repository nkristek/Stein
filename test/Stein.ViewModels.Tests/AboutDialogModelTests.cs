using NKristek.Smaragd.Commands;
using System;
using Xunit;

namespace Stein.ViewModels.Tests
{
    public class AboutDialogModelTests
    {
        [Fact]
        public void Dependencies_not_null()
        {
            var dialogModel = new AboutDialogModel();
            Assert.NotNull(dialogModel.Dependencies);
        }

        [Fact]
        public void Name_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.Name, "test");
        }

        [Fact]
        public void Description_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.Description, "test");
        }

        [Fact]
        public void Version_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.Version, new Version(1, 2, 3, 4));
        }

        [Fact]
        public void Copyright_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.Copyright, "test");
        }

        [Fact]
        public void AdditionalNotes_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.AdditionalNotes, "test");
        }

        [Fact]
        public void Uri_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.Uri, new Uri("http://www.example.com"));
        }

        [Fact]
        public void Publisher_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.Publisher, "test");
        }

        private class TestCommand
            : ViewModelCommand<AboutDialogModel>
        {
            protected override void Execute(AboutDialogModel viewModel, object parameter)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void OpenUriCommand_property()
        {
            var dialogModel = new AboutDialogModel();
            dialogModel.TestProperty(() => dialogModel.OpenUriCommand, new TestCommand(), PropertyTestSettings.IsDirtyIgnored | PropertyTestSettings.IsReadOnlyIgnored);
        }
    }
}
