using System.Linq;
using System.Windows;
using nkristek.Stein.Services;
using nkristek.Stein.ViewModels;

namespace nkristek.Stein
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel(null, this);
            DataContext = viewModel;

            viewModel.RefreshApplicationsCommand.Execute(null);

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null)
                return;

            // save changes from application viewmodels back to the configuration
            foreach (var changedApplication in viewModel.Applications.Where(application => application.IsDirty))
                ViewModelService.SaveViewModel(changedApplication);
        }
    }
}
