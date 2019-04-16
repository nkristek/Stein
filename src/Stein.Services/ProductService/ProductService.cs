using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Stein.Services.ProductService
{
    /// <inheritdoc />
    public class ProductService
        : IProductService
    {
        /// <inheritdoc />
        public IEnumerable<IProduct> GetInstalledProducts()
        {
            return ReadProducts().ToList();
        }
        
        private readonly HashSet<string> _installedProductCodes = new HashSet<string>();

        /// <inheritdoc />
        public void RefreshInstalledProducts()
        {
            _installedProductCodes.Clear();
            foreach (var product in ReadProducts())
            {
                using (product)
                {
                    var productCode = product.ProductCode;
                    if (!String.IsNullOrEmpty(productCode)
                        && !_installedProductCodes.Contains(productCode))
                        _installedProductCodes.Add(productCode);
                }
            }
        }

        private static IEnumerable<IProduct> ReadProducts()
        {
            const string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            using (var key = Registry.LocalMachine.OpenSubKey(registryPath))
                foreach (var program in GetProductsFromKey(key))
                    yield return program;

            using (var key = Registry.CurrentUser.OpenSubKey(registryPath))
                foreach (var program in GetProductsFromKey(key))
                    yield return program;

            if (!Environment.Is64BitOperatingSystem)
                yield break;

            const string wow6432NodeRegistryPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            using (var key = Registry.LocalMachine.OpenSubKey(wow6432NodeRegistryPath))
                foreach (var program in GetProductsFromKey(key))
                    yield return program;
        }

        private static IEnumerable<IProduct> GetProductsFromKey(RegistryKey key)
        {
            return key.GetSubKeyNames().Select(subKey => new Product(key.OpenSubKey(subKey)));
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">If <paramref name="productCode"/> is <c>null</c> or empty.</exception>
        public bool IsProductInstalled(string productCode)
        {
            if (String.IsNullOrEmpty(productCode))
                throw new ArgumentNullException(nameof(productCode));

            return _installedProductCodes.Contains(productCode);
        }
    }
}
