using System;
using System.Collections.Generic;
using Microsoft.Deployment.WindowsInstaller;
using Stein.Common.MsiService;
using Stein.Utility;

namespace Stein.Services.MsiService
{
    /// <inheritdoc cref="IMsiMetadata" />
    public class MsiMetadata
        : Disposable, IMsiMetadata
    {
        private readonly Database _database;

        public MsiMetadata(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            _database = new Database(fileName, DatabaseOpenMode.ReadOnly);
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetAllProperties()
        {
            var properties = new Dictionary<string, string>();
            using (var view = _database.OpenView(_database.Tables["Property"].SqlSelectString))
            {
                view.Execute();
                foreach (var record in view) using (record)
                    properties[record.GetString("Property")] = record.GetString("Value");
            }
            return properties;
        }

        /// <inheritdoc />
        public string GetProperty(MsiPropertyName propertyName)
        {
            return _database.ExecutePropertyQuery(propertyName.ToString());
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}
