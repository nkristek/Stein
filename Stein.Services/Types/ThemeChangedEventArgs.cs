using System;
namespace Stein.Services.Types
{
    public class ThemeChangedEventArgs
        : EventArgs
    {
        public Theme OldTheme { get; }

        public Theme NewTheme { get; }

        public ThemeChangedEventArgs(Theme oldTheme, Theme newTheme)
        {
            OldTheme = oldTheme;
            NewTheme = newTheme;
        }
    }
}
