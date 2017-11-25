using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfBase.Commands;

namespace WpfBase.ViewModels
{
    /// <summary>
    /// Adds the functionality to use the PropertySourceAttribute above properties and CommandCanExecuteSourceAttribute above ViewModelCommand or AsyncViewModelCommand implementations
    /// </summary>
    public abstract class ComputedBindableBase
        : BindableBase
    {
        public ComputedBindableBase()
        {
            var declaredProperties = GetType().GetTypeInfo().DeclaredProperties;

            // PropertySourceAttribute
            var propertiesWithPropertiesToNotify = new Dictionary<string, HashSet<string>>();
            foreach (var property in declaredProperties)
            {
                // get the PropertySource attribute from the property, if it exists this property should be notified from the source properties listed in the attribute
                var computedAttribute = property.GetCustomAttribute<PropertySourceAttribute>();
                if (computedAttribute == null)
                    continue;

                foreach (var sourceName in computedAttribute.Sources)
                {
                    // skip when there is no property with this name
                    if (!declaredProperties.Select(p => p.Name).Contains(sourceName))
                        continue;

                    // create a new entry in the dictionary if this property doesn't notify another property already
                    if (!propertiesWithPropertiesToNotify.ContainsKey(sourceName))
                        propertiesWithPropertiesToNotify[sourceName] = new HashSet<string>();

                    // add the property to the list of properties which get notified
                    propertiesWithPropertiesToNotify[sourceName].Add(property.Name);
                }
            }
            
            // CommandCanExecuteSourceAttribute
            var propertiesWithCommandsToNotify = new Dictionary<string, HashSet<string>>();
            foreach (var property in declaredProperties)
            {
                // get the CommandCanExecuteSource attribute from the property, if it exists this command should be notified from the source properties listed in the attribute
                var computedAttribute = property.GetCustomAttribute<CommandCanExecuteSourceAttribute>();
                if (computedAttribute == null)
                    continue;

                foreach (var sourceName in computedAttribute.Sources)
                {
                    // skip when there is no property with this name
                    if (!declaredProperties.Select(p => p.Name).Contains(sourceName))
                        continue;

                    // create a new entry in the dictionary if this property doesn't notify another command already
                    if (!propertiesWithCommandsToNotify.ContainsKey(sourceName))
                        propertiesWithCommandsToNotify[sourceName] = new HashSet<string>();

                    // add the command to the list of commands which get notified
                    propertiesWithCommandsToNotify[sourceName].Add(property.Name);
                }
            }

            PropertyChanged += (sender, e) => {
                if (propertiesWithPropertiesToNotify.ContainsKey(e.PropertyName))
                {
                    foreach (var propertyNameToNotify in propertiesWithPropertiesToNotify[e.PropertyName])
                        RaisePropertyChanged(propertyNameToNotify);
                }

                if (propertiesWithCommandsToNotify.ContainsKey(e.PropertyName))
                {
                    var type = GetType();
                    foreach (var commandNameToNotify in propertiesWithCommandsToNotify[e.PropertyName])
                    {
                        try
                        {
                            var field = type.GetProperty(commandNameToNotify);
                            if (field == null)
                                continue;

                            var value = field.GetValue(this);
                            if (value is BindableCommand)
                                (value as BindableCommand).RaiseCanExecuteChanged();
                            else if (value is AsyncBindableCommand)
                                (value as AsyncBindableCommand).RaiseCanExecuteChanged();
                        }
                        catch { }
                    }
                }
            };
        }
    }
}
