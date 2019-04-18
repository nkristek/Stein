using System.Windows;
using System.Windows.Input;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for InstallationResultDialog.xaml
    /// </summary>
    public partial class InstallationResultDialog : Dialog
    {
        public InstallationResultDialog()
        {
            InitializeComponent();

            KeyDown += Dialog_KeyDown;
        }

        private void Dialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (!OkButton.IsEnabled || e.Key != Key.Enter)
                return;

            e.Handled = true;
            DialogResult = true;
        }

        private void OnDialogOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
