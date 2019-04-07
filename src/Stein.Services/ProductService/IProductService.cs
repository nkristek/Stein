using System.Collections.Generic;

namespace Stein.Services.ProductService
{
    /// <summary>
    /// Defines methods for determining the status of installed products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get the list of installed products from the registry.
        /// </summary>
        /// <returns>Installed products.</returns>
        IEnumerable<IProduct> GetInstalledProducts();

        /// <summary>
        /// Refreshes the list of installed products.
        /// </summary>
        void RefreshInstalledProducts();

        /// <summary>
        /// Determines if a product is installed by searching the <paramref name="productCode"/> in the installed products.
        /// </summary>
        /// <param name="productCode">The ProductCode of the product.</param>
        /// <returns>If the product is installed.</returns>
        bool IsProductInstalled(string productCode);
    }
}
