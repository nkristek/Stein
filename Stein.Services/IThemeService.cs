using Stein.Services.Types;

namespace Stein.Services
{
    public interface IThemeService
    {
        Theme CurrentTheme { get; }

        void SetTheme(Theme theme);

        event ThemeChangedEventHandler ThemeChanged;
    }
}
