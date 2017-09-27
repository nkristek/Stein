using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfBase.ViewModels
{
    public abstract class ComputedBindableBase
        : BindableBase
    {
        public ComputedBindableBase()
        {
            var propertiesWithPropertiesToNotify = new Dictionary<string, HashSet<string>>();
            var declaredProperties = GetType().GetTypeInfo().DeclaredProperties;
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

                    // add the property to the list of property which get notified
                    propertiesWithPropertiesToNotify[sourceName].Add(property.Name);
                }
            }

            PropertyChanged += (sender, e) => {
                if (!propertiesWithPropertiesToNotify.ContainsKey(e.PropertyName))
                    return;

                foreach (var propertyNameToNotify in propertiesWithPropertiesToNotify[e.PropertyName])
                    OnPropertyChanged(propertyNameToNotify);
            };
        }
    }
}
