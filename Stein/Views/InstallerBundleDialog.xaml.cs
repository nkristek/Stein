using System.Windows;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for SelectInstallersDialog.xaml
    /// </summary>
    public partial class InstallerBundleDialog : Window
    {
        public InstallerBundleDialog()
        {
            InitializeComponent();
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
