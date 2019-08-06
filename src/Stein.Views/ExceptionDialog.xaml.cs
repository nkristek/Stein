using System.Windows;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for ExceptionDialog.xaml
    /// </summary>
    public partial class ExceptionDialog : Dialog
    {
        public ExceptionDialog()
        {
            InitializeComponent();
        }

        private void OnDialogOkButtonClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
        }
    }
}
