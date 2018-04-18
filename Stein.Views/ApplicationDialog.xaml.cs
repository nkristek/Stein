using System.Windows;
using System.Windows.Input;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for ApplicationDialog.xaml
    /// </summary>
    public partial class ApplicationDialog : Window
    {
        public ApplicationDialog()
        {
            InitializeComponent();

            KeyDown += ApplicationDialog_KeyDown;
        }

        private void ApplicationDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (OkButton.IsEnabled && e.Key == Key.Enter)
            {
                e.Handled = true;
                DialogResult = true;
            }
        }

        private void OnDialogOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnDialogCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
