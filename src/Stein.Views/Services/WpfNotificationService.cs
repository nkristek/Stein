using Stein.Presentation;
using Stein.Utility;
using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Stein.Views.Services
{
    /// <inheritdoc />
    public class WpfNotificationService
        : Disposable, INotificationService
    {
        private readonly Notifier _notifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfNotificationService"/> class with a duration of 5 seconds.
        /// </summary>
        public WpfNotificationService() : this(TimeSpan.FromSeconds(5))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfNotificationService"/> class with the specified duration.
        /// </summary>
        /// <param name="duration">Duration to show each message.</param>
        public WpfNotificationService(TimeSpan duration)
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.Dispatcher = Application.Current.Dispatcher;

                cfg.PositionProvider = new PrimaryScreenPositionProvider(
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: duration,
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.DisplayOptions.TopMost = true;
            });
        }

        /// <inheritdoc />
        protected override void Dispose(bool managed = true)
        {
            if (managed)
                _notifier?.Dispose();
        }

        /// <inheritdoc />
        public void ShowInfo(string message, Action onClick = null)
        {
            _notifier.ShowInformation(message, new MessageOptions
            {
                ShowCloseButton = true,
                NotificationClickAction = notification => 
                {
                    notification?.Close();
                    onClick?.Invoke();
                },
                FreezeOnMouseEnter = true,
                UnfreezeOnMouseLeave = true
            });
        }

        /// <inheritdoc />
        public void ShowSuccess(string message, Action onClick = null)
        {
            _notifier.ShowSuccess(message, new MessageOptions
            {
                ShowCloseButton = true,
                NotificationClickAction = notification =>
                {
                    notification?.Close();
                    onClick?.Invoke();
                },
                FreezeOnMouseEnter = true,
                UnfreezeOnMouseLeave = true
            });
        }

        /// <inheritdoc />
        public void ShowWarning(string message, Action onClick = null)
        {
            _notifier.ShowWarning(message, new MessageOptions
            {
                ShowCloseButton = true,
                NotificationClickAction = notification =>
                {
                    notification?.Close();
                    onClick?.Invoke();
                },
                FreezeOnMouseEnter = true,
                UnfreezeOnMouseLeave = true
            });
        }

        /// <inheritdoc />
        public void ShowError(string message, Action onClick = null)
        {
            _notifier.ShowError(message, new MessageOptions
            {
                ShowCloseButton = true,
                NotificationClickAction = notification =>
                {
                    notification?.Close();
                    onClick?.Invoke();
                },
                FreezeOnMouseEnter = true,
                UnfreezeOnMouseLeave = true
            });
        }
    }
}
