using System;
using System.Collections.Generic;

namespace WpfBase.ViewModels
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandCanExecuteSourceAttribute
        : Attribute
    {
        public IEnumerable<string> Sources { get; private set; }

        public CommandCanExecuteSourceAttribute(params string[] sources)
        {
            Sources = sources;
        }
    }
}
