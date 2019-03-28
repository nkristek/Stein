using System.Collections.ObjectModel;
using NKristek.Smaragd.ViewModels;

namespace Stein.ViewModels
{
    public sealed class InstallationResultDialogModel
        : DialogModel
    {
        /// <summary>
        /// The results of each operation.
        /// </summary>
        public ObservableCollection<InstallationResultViewModel> InstallationResults { get; } = new ObservableCollection<InstallationResultViewModel>();
    }
}
