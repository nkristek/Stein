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
            return ReadInstalledPrograms().ToList();
        }

        private static IEnumerable<IProduct> ReadInstalledPrograms()
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

            return ReadInstalledPrograms().Any(program => !String.IsNullOrEmpty(program.UninstallString) && program.UninstallString.Contains(productCode));
        }
    }
}
