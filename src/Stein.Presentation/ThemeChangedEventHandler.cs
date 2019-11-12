namespace Stein.Presentation
{
    /// <summary>Represents the method that will handle the <see cref="IThemeService.ThemeChanged" /> event raised when a theme is changed.</summary>
    /// <param name="sender">The source of the event. </param>
    /// <param name="e">A <see cref="ThemeChangedEventArgs" /> that contains the event data. </param>
    public delegate void ThemeChangedEventHandler(object? sender, ThemeChangedEventArgs? e);
}
