using System.Collections.Generic;
using Microsoft.Deployment.WindowsInstaller;

namespace Stein.Services.MsiService
{
    /// <inheritdoc />
    public class MsiService
        : IMsiService
    {
        /// <inheritdoc />
        public Database GetMsiDatabase(string fileName)
        {
            return new Database(fileName, DatabaseOpenMode.ReadOnly);
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetAllPropertiesFromMsi(string fileName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetAllPropertiesFromMsiDatabase(database);
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetAllPropertiesFromMsiDatabase(Database database)
        {
            var properties = new Dictionary<string, string>();

            using (var view = database.OpenView(database.Tables["Property"].SqlSelectString))
            {
                view.Execute();
                foreach (var record in view) using (record)
                    properties[record.GetString("Property")] = record.GetString("Value");
            }

            return properties;
        }

        /// <inheritdoc />
        public string GetPropertyFromMsi(string fileName, MsiPropertyName propertyName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetPropertyFromMsiDatabase(database, propertyName);
        }

        /// <inheritdoc />
        public string GetPropertyFromMsiDatabase(Database database, MsiPropertyName propertyName)
        {
            return database.ExecutePropertyQuery(propertyName.ToString());
        }
    }
}
