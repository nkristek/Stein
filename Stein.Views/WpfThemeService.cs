using Stein.Services;
using Stein.Services.Types;

namespace Stein.Views
{
    public class WpfThemeService
        : IThemeService
    {
        private Theme _currentTheme;

        public Theme CurrentTheme
        {
            get => _currentTheme;
            set
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

            // do theming

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
