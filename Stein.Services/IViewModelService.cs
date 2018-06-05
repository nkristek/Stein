using System.Collections.Generic;
using NKristek.Smaragd.ViewModels;

namespace Stein.Services
{
    public interface IViewModelService
    {
        /// <summary>
        /// Create a <see cref="ViewModel"/> from the corresponding entity
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="ViewModel"/></typeparam>
        /// <param name="parent">Parent of the <see cref="ViewModel"/> (optional)</param>
        /// <returns>The created <see cref="ViewModel"/></returns>
        TViewModel CreateViewModel<TViewModel>(ViewModel parent = null) where TViewModel : ViewModel;

        /// <summary>
        /// Create all <see cref="ViewModel"/> from entities
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="ViewModel"/></typeparam>
        /// <param name="parent">Parent of all <see cref="ViewModel"/> (optional)</param>
        /// <param name="existingViewModels">Existing <see cref="ViewModel"/> which should get reused</param>
        /// <returns>All available <see cref="ViewModel"/></returns>
        IEnumerable<TViewModel> CreateViewModels<TViewModel>(ViewModel parent = null, IEnumerable<TViewModel> existingViewModels = null) where TViewModel : ViewModel;

        /// <summary>
        /// Saves a <see cref="ViewModel"/> to the persistent entity
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="ViewModel"/></typeparam>
        /// <param name="viewModel"><see cref="ViewModel"/> to save</param>
        void SaveViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel;

        /// <summary>
        /// Updates the given <see cref="ViewModel"/> and discard any changes made to it
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="ViewModel"/></typeparam>
        /// <param name="viewModel"><see cref="ViewModel"/> to update</param>
        void UpdateViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel;

        /// <summary>
        /// Deletes the corresponding persistent entity
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="ViewModel"/></typeparam>
        /// <param name="viewModel"><see cref="ViewModel"/> to delete</param>
        void DeleteViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel;
    }
}
