using Stein.ViewModels;
using Stein.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBase.ViewModels;

namespace Stein.Services
{
    public static class DialogService
    {
        private static readonly IReadOnlyDictionary<Type, Type> ViewModelsToViewsMapping = new Dictionary<Type, Type>
        {
            { typeof(InstallerBundleViewModel), typeof(SelectInstallersDialog) }
        };

        public static bool? ShowDialog(ViewModel dialogViewModel, string title = null)
        {
            var viewModelType = dialogViewModel.GetType();
            if (!ViewModelsToViewsMapping.ContainsKey(viewModelType))
                throw new NotSupportedException("No view found for viewmodel");

            var dialog = Activator.CreateInstance(ViewModelsToViewsMapping[viewModelType]) as Window;
            if (dialog == null)
                throw new ArgumentException("view for viewmodel is no window");

            dialog.Title = title ?? String.Empty;
            dialog.DataContext = dialogViewModel;
            dialog.Owner = dialogViewModel.Parent?.View as Window;

            dialogViewModel.View = dialog;

            return dialog.ShowDialog();
        }
    }
}
