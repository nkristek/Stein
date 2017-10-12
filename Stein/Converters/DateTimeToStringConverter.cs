using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Stein.Converters
{
    public class DateTimeToStringConverter
        : IValueConverter
    {
        public static DateTimeToStringConverter Instance = new DateTimeToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            if (dateTime == DateTime.MinValue)
                return "Date not set";
            return String.Join(" ", dateTime.ToShortDateString(), dateTime.ToShortTimeString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
