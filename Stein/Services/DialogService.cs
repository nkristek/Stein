using System;
using System.Windows;
using System.Collections.Generic;
using nkristek.MVVMBase.ViewModels;
using nkristek.Stein.Localizations;
using nkristek.Stein.ViewModels;
using nkristek.Stein.Views;

namespace nkristek.Stein.Services
{
    public static class DialogService
    {
        /// <summary>
        /// Mapping of the ViewModels to Views
        /// </summary>
        private static readonly IReadOnlyDictionary<Type, Type> ViewModelsToViewsMapping = new Dictionary<Type, Type>
        {
            { typeof(InstallerBundleViewModel), typeof(InstallerBundleDialog) },
            { typeof(ApplicationViewModel), typeof(ApplicationDialog) }
        };

        /// <summary>
        /// Shows the corresponding dialog of the ViewModel
        /// </summary>
        /// <param name="dialogViewModel">A ViewModel which has a dialog</param>
        /// <param name="title">Title of the window</param>
        /// <returns>The result of the dialog</returns>
        public static bool? ShowDialog(ViewModel dialogViewModel, string title = null)
        {
            var viewModelType = dialogViewModel.GetType();
            if (!ViewModelsToViewsMapping.ContainsKey(viewModelType))
                throw new NotSupportedException(Strings.NoViewExistsForViewModel);

            var dialog = Activator.CreateInstance(ViewModelsToViewsMapping[viewModelType]) as Window;
            if (dialog == null)
                throw new ArgumentException(Strings.ViewIsNoWindow);

            dialog.Title = title ?? String.Empty;
            dialog.DataContext = dialogViewModel;
            dialog.Owner = dialogViewModel.Parent?.View as Window;

            dialogViewModel.View = dialog;
            var dialogResult = dialog.ShowDialog();
            dialogViewModel.View = null;

            return dialogResult;
        }
    }
}
