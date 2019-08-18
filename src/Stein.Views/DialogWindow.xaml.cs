using AdonisUI.Controls;
using Stein.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Stein.Views
{
    public partial class DialogWindow : AdonisWindow
    {
        public DialogWindow()
        {
            InitializeComponent();

            PreviewKeyDown += Dialog_PreviewKeyDown;
        }

        private int _konamiCodeMatch;

        private readonly List<Key> _konamiCode = new List<Key>
        {
            Key.Up,
            Key.Up,
            Key.Down,
            Key.Down,
            Key.Left,
            Key.Right,
            Key.Left,
            Key.Right,
            Key.B,
            Key.A,
        };

        private void Dialog_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HandleKonamiCode(e);
        }

        private void HandleKonamiCode(KeyEventArgs e)
        {
            if (_konamiCodeMatch >= _konamiCode.Count || _konamiCodeMatch < 0)
                _konamiCodeMatch = 0;

            if (e.Key == _konamiCode[_konamiCodeMatch])
                _konamiCodeMatch++;
            else
                _konamiCodeMatch = 0;

            if (_konamiCodeMatch >= _konamiCode.Count
                && DataContext is AboutDialogModel aboutDialogModel
                && aboutDialogModel.Parent is MainWindowDialogModel mainWindowDialogModel)
                mainWindowDialogModel.ChangeThemeCommand.Execute(nameof(Presentation.Theme.HotDog));
        }
    }
}
