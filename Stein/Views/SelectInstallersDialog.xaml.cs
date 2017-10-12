using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for SelectInstallersDialog.xaml
    /// </summary>
    public partial class SelectInstallersDialog : Window
    {
        public SelectInstallersDialog()
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
