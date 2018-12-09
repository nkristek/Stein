using System.Collections.Generic;
using NKristek.Smaragd.ViewModels;

namespace Stein.Services
{
    public interface IViewModelService
    {
        /// <summary>
        /// Create a <see cref="IViewModel"/> from the corresponding entity.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be created.</typeparam>
        /// <param name="parent">Parent of the <see cref="IViewModel"/> (optional)</param>
        /// <returns>The created <see cref="IViewModel"/></returns>
        TViewModel CreateViewModel<TViewModel>(IViewModel parent = null) where TViewModel : class, IViewModel;

        /// <summary>
        /// Create all <see cref="IViewModel"/> from entities.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be created.</typeparam>
        /// <param name="parent">Parent of all <see cref="IViewModel"/> (optional)</param>
        /// <param name="existingViewModels">Existing <see cref="IViewModel"/> which should get reused</param>
        /// <returns>All available <see cref="IViewModel"/></returns>
        IEnumerable<TViewModel> CreateViewModels<TViewModel>(IViewModel parent = null, IEnumerable<TViewModel> existingViewModels = null) where TViewModel : class, IViewModel;

        /// <summary>
        /// Saves a <see cref="IViewModel"/> to the persistent entity.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be saved.</typeparam>
        /// <param name="viewModel"><see cref="IViewModel"/> to save</param>
        void SaveViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;

        /// <summary>
        /// Updates the given <see cref="IViewModel"/> and discard any changes made to it.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be updated.</typeparam>
        /// <param name="viewModel"><see cref="IViewModel"/> to update</param>
        void UpdateViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;

        /// <summary>
        /// Deletes the corresponding persistent entity.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be deleted.</typeparam>
        /// <param name="viewModel"><see cref="IViewModel"/> to delete</param>
        void DeleteViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
    }
}
