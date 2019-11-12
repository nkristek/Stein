using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Stein.Views.Resources
{
    /// <inheritdoc cref="Behavior{T}" />
    /// <summary>
    /// Taken from https://stackoverflow.com/a/44253691
    /// </summary>
    internal class PeriodicBindingUpdateBehavior
        : Behavior<DependencyObject>, IDisposable
    {
        public TimeSpan Interval { get; set; }

        public DependencyProperty Property { get; set; }

        public PeriodicBindingUpdateMode Mode { get; set; } = PeriodicBindingUpdateMode.UpdateTarget;

        private WeakTimer _timer;

        private TimerCallback _timerCallback;

        protected override void OnAttached()
        {
            if (Interval == default)
                throw new ArgumentNullException(nameof(Interval));
            if (Interval < TimeSpan.Zero)
                throw new ArgumentException("Negative intervals are not supported.", nameof(Interval));
            if (Property == null)
                throw new ArgumentNullException(nameof(Property));

            // Save a reference to the callback of the timer so this object will keep the timer alive but not vice versa.
            _timerCallback = s =>
            {
                try
                {
                    switch (Mode)
                    {
                        case PeriodicBindingUpdateMode.UpdateTarget:
                            Dispatcher.Invoke(() => BindingOperations.GetBindingExpression(AssociatedObject, Property)?.UpdateTarget());
                            break;
                        case PeriodicBindingUpdateMode.UpdateSource:
                            Dispatcher.Invoke(() => BindingOperations.GetBindingExpression(AssociatedObject, Property)?.UpdateSource());
                            break;
                    }
                }
                catch (TaskCanceledException) { }//This exception will be thrown when application is shutting down.
            };
            _timer = new WeakTimer(_timerCallback, null, Interval, Interval);

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            Dispose();
            base.OnDetaching();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _timer?.Dispose();
            _timerCallback = null;
        }
    }
}
