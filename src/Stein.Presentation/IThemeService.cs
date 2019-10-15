namespace Stein.Presentation
{
    public interface IThemeService
    {
        Theme CurrentTheme { get; }

        void SetTheme(Theme theme);

        event ThemeChangedEventHandler? ThemeChanged;
    }
}
