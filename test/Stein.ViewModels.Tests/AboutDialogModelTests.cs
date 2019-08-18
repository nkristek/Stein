using NKristek.Smaragd.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Name_set_get()
        {
            var testData = "Test";
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.Name), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.Name), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.Name = testData;
            Assert.Equal(testData, dialogModel.Name);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }

        [Fact]
        public void Description_set_get()
        {
            const string testData = "Test";
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.Description), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.Description), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.Description = testData;
            Assert.Equal(testData, dialogModel.Description);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }

        [Fact]
        public void Version_set_get()
        {
            var testData = new Version(1,2,3,4);
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.Version), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.Version), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.Version = testData;
            Assert.Equal(testData, dialogModel.Version);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }

        [Fact]
        public void Copyright_set_get()
        {
            var testData = "Test";
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.Copyright), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.Copyright), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.Copyright = testData;
            Assert.Equal(testData, dialogModel.Copyright);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }

        [Fact]
        public void AdditionalNotes_set_get()
        {
            var testData = "Test";
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.AdditionalNotes), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.AdditionalNotes), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.AdditionalNotes = testData;
            Assert.Equal(testData, dialogModel.AdditionalNotes);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }

        [Fact]
        public void Uri_set_get()
        {
            var testData = new Uri("http://www.example.com");
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.Uri), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.Uri), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.Uri = testData;
            Assert.Equal(testData, dialogModel.Uri);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }

        [Fact]
        public void Publisher_set_get()
        {
            var testData = "Test";
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.Publisher), nameof(dialogModel.IsDirty) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.Publisher), nameof(dialogModel.IsDirty) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.Publisher = testData;
            Assert.Equal(testData, dialogModel.Publisher);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
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
        public void OpenUriCommand_set_get()
        {
            var testData = new TestCommand();
            var dialogModel = new AboutDialogModel();
            var invokedPropertyChangingEvents = new List<string>();
            var expectedPropertyChangingEvents = new List<string> { nameof(dialogModel.OpenUriCommand) };
            var invokedPropertyChangedEvents = new List<string>();
            var expectedPropertyChangedEvents = new List<string> { nameof(dialogModel.OpenUriCommand) };
            dialogModel.PropertyChanging += (sender, args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            dialogModel.PropertyChanged += (sender, args) => invokedPropertyChangedEvents.Add(args.PropertyName);

            dialogModel.OpenUriCommand = testData;
            Assert.Equal(testData, dialogModel.OpenUriCommand);
            Assert.Equal(expectedPropertyChangingEvents.OrderBy(e => e), invokedPropertyChangingEvents.OrderBy(e => e));
            Assert.Equal(expectedPropertyChangedEvents.OrderBy(e => e), invokedPropertyChangedEvents.OrderBy(e => e));
        }
    }
}
