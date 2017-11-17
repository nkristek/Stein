using System;

namespace Stein.Configuration
{
    public class Date
    {
        public Date() { }

        public Date(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; set; }
    }
}
