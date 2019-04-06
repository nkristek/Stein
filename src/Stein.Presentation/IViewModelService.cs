using System.Threading.Tasks;
using NKristek.Smaragd.ViewModels;

namespace Stein.Presentation
{
    public interface IViewModelService
    {
        /// <summary>
        /// Create a <see cref="IViewModel"/> from the corresponding entity.
        /// Please note, that no asynchronous work will be executed and you have to manually call <see cref="UpdateViewModelAsync{TViewModel}"/> for the <see cref="TViewModel"/> to get populated with data.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be created.</typeparam>
        /// <param name="parent">Parent of the <see cref="IViewModel"/> (optional)</param>
        /// <param name="entity">Entity of the <see cref="IViewModel"/> (optional)</param>
        /// <returns>The created <see cref="IViewModel"/></returns>
        TViewModel CreateViewModel<TViewModel>(IViewModel parent = null, object entity = null) where TViewModel : class, IViewModel;

        /// <summary>
        /// Saves a <see cref="IViewModel"/> to the persistent entity asynchronously.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be saved.</typeparam>
        /// <param name="viewModel"><see cref="IViewModel"/> to save</param>
        /// <param name="entity">Entity of the <see cref="IViewModel"/> (optional)</param>
        Task SaveViewModelAsync<TViewModel>(TViewModel viewModel, object entity = null) where TViewModel : class, IViewModel;

        /// <summary>
        /// Updates the given <see cref="IViewModel"/> and discard any changes made to it asynchronously.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be updated.</typeparam>
        /// <param name="viewModel"><see cref="IViewModel"/> to update</param>
        /// <param name="entity">Entity of the <see cref="IViewModel"/> (optional)</param>
        Task UpdateViewModelAsync<TViewModel>(TViewModel viewModel, object entity = null) where TViewModel : class, IViewModel;

        /// <summary>
        /// Deletes the corresponding persistent entity asynchronously.
        /// </summary>
        /// <typeparam name="TViewModel">Subclass of <see cref="IViewModel"/> which should be deleted.</typeparam>
        /// <param name="viewModel"><see cref="IViewModel"/> to delete</param>
        /// <param name="entity">Entity of the <see cref="IViewModel"/> (optional)</param>
        Task DeleteViewModelAsync<TViewModel>(TViewModel viewModel, object entity = null) where TViewModel : class, IViewModel;
    }
}
