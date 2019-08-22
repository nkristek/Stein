using System;
using System.Windows;
using AdonisUI;
using Stein.Presentation;

namespace Stein.Views.Services
{
    /// <inheritdoc />
    public class WpfThemeService
        : IThemeService
    {
        private Theme _currentTheme;

        /// <inheritdoc />
        public Theme CurrentTheme
        {
            get => _currentTheme;
            private set
            {
                if (value == _currentTheme)
                    return;

                var oldValue = _currentTheme;
                _currentTheme = value;
                
                NotifyThemeChanged(oldValue, value);
            }
        }

        /// <inheritdoc />
        public void SetTheme(Theme theme)
        {
            if (theme == CurrentTheme)
                return;

            ResourceLocator.SetColorScheme(Application.Current.Resources, GetColorScheme(theme), GetColorScheme(CurrentTheme));
            CurrentTheme = theme;
        }

        private static Uri GetColorScheme(Theme theme)
        {
            switch (theme)
            {
                case Theme.Light: return ResourceLocator.LightColorScheme;
                case Theme.Dark: return ResourceLocator.DarkColorScheme;
                case Theme.HotDog: return new Uri("pack://application:,,,/Stein.Views;component/Resources/HotDog.xaml", UriKind.Absolute);
                default: throw new NotSupportedException("This theme is not supported");
            }
        }

        /// <inheritdoc />
        public event ThemeChangedEventHandler ThemeChanged;

        /// <summary>
        /// Raises an event on the <see cref="ThemeChangedEventHandler"/> to indicate that the <see cref="CurrentTheme"/> changed.
        /// </summary>
        protected virtual void NotifyThemeChanged(Theme oldTheme, Theme newTheme)
        {
            ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(oldTheme, newTheme));
        }
    }
}
