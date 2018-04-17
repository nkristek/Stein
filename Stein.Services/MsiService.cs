using System.Collections.Generic;
using Microsoft.Deployment.WindowsInstaller;
using Stein.Services.Types;

namespace Stein.Services
{
    public class MsiService
        : IMsiService
    {
        public static IMsiService Instance;

        public Database GetMsiDatabase(string fileName)
        {
            try
            {
                return new Database(fileName, DatabaseOpenMode.ReadOnly);
            }
            catch
            {
                return null;
            }
        }
        
        public Dictionary<string, string> GetAllPropertiesFromMsi(string fileName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetAllPropertiesFromMsiDatabase(database);
        }
        
        public Dictionary<string, string> GetAllPropertiesFromMsiDatabase(Database database)
        {
            try
            {
                var properties = new Dictionary<string, string>();

                using (var view = database.OpenView(database.Tables["Property"].SqlSelectString))
                {
                    view.Execute();
                    foreach (var record in view) using (record)
                        properties.Add(record.GetString("Property"), record.GetString("Value"));
                }

                return properties;
            }
            catch
            {
                return null;
            }
        }
        
        public string GetPropertyFromMsi(string fileName, MsiPropertyName propertyName)
        {
            using (var database = GetMsiDatabase(fileName))
                return GetPropertyFromMsiDatabase(database, propertyName);
        }
        
        public string GetPropertyFromMsiDatabase(Database database, MsiPropertyName propertyName)
        {
            try
            {
                return database.ExecutePropertyQuery(propertyName.ToString());
            }
            catch
            {
                return null;
            }
        }
    }
}
