using System.Windows;
using System.Windows.Input;

namespace Stein.Views
{
    public partial class AboutDialog : Dialog
    {
        public AboutDialog()
        {
            InitializeComponent();

            KeyDown += Dialog_KeyDown;
        }

        private void Dialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (!OkButton.IsEnabled)
                        break;
                    e.Handled = true;
                    Window.GetWindow(this).DialogResult = true;
                    break;
                case Key.Escape:
                    e.Handled = true;
                    Window.GetWindow(this).DialogResult = false;
                    break;
                default:
                    break;
            }
        }

        private void OnDialogOkButtonClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
        }
    }
}
