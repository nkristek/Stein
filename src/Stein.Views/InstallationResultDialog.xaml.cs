using System.Windows;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for InstallationResultDialog.xaml
    /// </summary>
    public partial class InstallationResultDialog : Window
    {
        public InstallationResultDialog()
        {
            InitializeComponent();
        }

        private void OnDialogOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
