using System;
using System.Windows;
using AdonisUI;
using Stein.Presentation;

namespace Stein.Views.Services
{
    public class WpfThemeService
        : IThemeService
    {
        private Theme _currentTheme;

        public Theme CurrentTheme
        {
            get => _currentTheme;
            private set
            {
                if (value == _currentTheme)
                    return;

                var oldValue = _currentTheme;
                _currentTheme = value;
                
                RaiseThemeChanged(oldValue, value);
            }
        }

        public void SetTheme(Theme theme)
        {
            if (theme == CurrentTheme)
                return;

            switch (theme)
            {
                case Theme.Light:
                    ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.LightColorScheme);
                    break;
                case Theme.Dark:
                    ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
                    break;
                default: throw new NotSupportedException("This theme is not supported");
            }
            
            CurrentTheme = theme;
        }

        public event ThemeChangedEventHandler ThemeChanged;

        /// <summary>
        /// Raises an event on the <see cref="ThemeChangedEventHandler"/>
        /// </summary>
        protected void RaiseThemeChanged(Theme oldTheme, Theme newTheme)
        {
            ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(oldTheme, newTheme));
        }
    }
}
