using System.Windows;
using System.Windows.Input;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Dialog
    {
        public UpdateDialog()
        {
            InitializeComponent();

            KeyDown += Dialog_KeyDown;
        }

        private void Dialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (OkButton.IsEnabled || e.Key != Key.Enter)
                return;

            e.Handled = true;
            Window.GetWindow(this).DialogResult = true;
        }

        private void OnDialogOkButtonClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
        }
    }
}
