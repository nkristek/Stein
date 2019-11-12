using NKristek.Smaragd.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Xunit;

namespace Stein.ViewModels.Tests
{
    [Flags]
    internal enum PropertyTestSettings
    {
        Default = 0,
        IsDirtyIgnored = 1,
        IsReadOnlyIgnored = 2
    }

    internal static class ViewModelExtensions
    {
        internal static void TestProperty<TViewModel, TProperty>(
            this TViewModel viewModel,
            Expression<Func<TProperty>> propertyExpression,
            TProperty testData,
            PropertyTestSettings settings = PropertyTestSettings.Default,
            TProperty defaultValue = default)
            where TViewModel : IViewModel
        {
            // check precondition
            Assert.NotEqual(defaultValue, testData);

            // init
            if (!(propertyExpression.Body is MemberExpression memberExpression))
                throw new ArgumentException("Expression body is not of type MemberExpression", nameof(propertyExpression));
            var propertyName = memberExpression.Member.Name;
            var propertyAccessor = new Accessor<TProperty>(propertyExpression);
            var propertyOldValue = propertyAccessor.Value;

            var isReadOnlyOldValue = viewModel.IsReadOnly;
            viewModel.IsReadOnly = false;
            Assert.False(viewModel.IsReadOnly);

            var isDirtyOldValue = viewModel.IsDirty;
            viewModel.IsDirty = false;
            Assert.False(viewModel.IsDirty);

            // check default value
            Assert.Equal(defaultValue, propertyAccessor.Value);

            // attach
            var invokedPropertyChangingEvents = new List<string>();
            var invokedPropertyChangedEvents = new List<string>();
            void propertyChanging(object sender, PropertyChangingEventArgs args) => invokedPropertyChangingEvents.Add(args.PropertyName);
            void propertyChanged(object sender, PropertyChangedEventArgs args) => invokedPropertyChangedEvents.Add(args.PropertyName);
            viewModel.PropertyChanging += propertyChanging;
            viewModel.PropertyChanged += propertyChanged;

            // set test data
            propertyAccessor.Value = testData;

            // check that the data has been set
            Assert.Equal(testData, propertyAccessor.Value);
            if (settings.HasFlag(PropertyTestSettings.IsDirtyIgnored))
                Assert.False(viewModel.IsDirty);
            else
                Assert.True(viewModel.IsDirty);

            // check INotifyPropertyChanging
            Assert.Contains(propertyName, invokedPropertyChangingEvents);
            if (settings.HasFlag(PropertyTestSettings.IsDirtyIgnored))
                Assert.DoesNotContain(nameof(IViewModel.IsDirty), invokedPropertyChangingEvents);
            else
                Assert.Contains(nameof(IViewModel.IsDirty), invokedPropertyChangingEvents);

            // check INotifyPropertyChanged
            Assert.Contains(propertyName, invokedPropertyChangedEvents);
            if (settings.HasFlag(PropertyTestSettings.IsDirtyIgnored))
                Assert.DoesNotContain(nameof(IViewModel.IsDirty), invokedPropertyChangedEvents);
            else
                Assert.Contains(nameof(IViewModel.IsDirty), invokedPropertyChangedEvents);

            // prepare for testing while isreadonly
            viewModel.IsReadOnly = true;
            Assert.True(viewModel.IsReadOnly);
            viewModel.IsDirty = false;
            Assert.False(viewModel.IsDirty);
            invokedPropertyChangingEvents.Clear();
            invokedPropertyChangedEvents.Clear();

            // set default value while isreadonly (which should be different from previous value)
            propertyAccessor.Value = defaultValue;

            // check if data has been set depending on isreadonlyignored
            if (settings.HasFlag(PropertyTestSettings.IsReadOnlyIgnored))
            {
                // check that the data has been set
                Assert.Equal(defaultValue, propertyAccessor.Value);
                if (settings.HasFlag(PropertyTestSettings.IsDirtyIgnored))
                    Assert.False(viewModel.IsDirty);
                else
                    Assert.True(viewModel.IsDirty);

                // check INotifyPropertyChanging
                Assert.Contains(propertyName, invokedPropertyChangingEvents);
                if (settings.HasFlag(PropertyTestSettings.IsDirtyIgnored))
                    Assert.DoesNotContain(nameof(IViewModel.IsDirty), invokedPropertyChangingEvents);
                else
                    Assert.Contains(nameof(IViewModel.IsDirty), invokedPropertyChangingEvents);

                // check INotifyPropertyChanged
                Assert.Contains(propertyName, invokedPropertyChangedEvents);
                if (settings.HasFlag(PropertyTestSettings.IsDirtyIgnored))
                    Assert.DoesNotContain(nameof(IViewModel.IsDirty), invokedPropertyChangedEvents);
                else
                    Assert.Contains(nameof(IViewModel.IsDirty), invokedPropertyChangedEvents);
            }
            else
            {
                // check that the data has NOT been set and the testData is still set
                Assert.Equal(testData, propertyAccessor.Value);
                Assert.False(viewModel.IsDirty);
                Assert.Empty(invokedPropertyChangingEvents);
                Assert.Empty(invokedPropertyChangedEvents);
            }

            // detach
            viewModel.PropertyChanging -= propertyChanging;
            viewModel.PropertyChanged -= propertyChanged;

            // set old data
            viewModel.IsReadOnly = false;
            propertyAccessor.Value = propertyOldValue;
            viewModel.IsReadOnly = isReadOnlyOldValue;
            viewModel.IsDirty = isDirtyOldValue;
        }
    }
}
