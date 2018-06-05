using System.Windows;
using System.Windows.Controls;
using Stein.ViewModels;

namespace Stein.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : UserControl
    {
        public MainWindowView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.RefreshApplicationsCommand.ExecuteAsync(null);
        }
    }
}
