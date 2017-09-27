using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBase.ViewModels
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertySourceAttribute
        : Attribute
    {
        public IEnumerable<string> Sources { get; private set; }

        public PropertySourceAttribute(params string[] sources)
        {
            Sources = sources;
        }
    }
}
