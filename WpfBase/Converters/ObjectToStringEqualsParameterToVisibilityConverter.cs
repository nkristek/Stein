using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfBase.Converters
{
    public class ObjectToStringEqualsParameterToVisibilityConverter
        : IValueConverter
    {
        public static ObjectToStringEqualsParameterToVisibilityConverter Instance = new ObjectToStringEqualsParameterToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterAsString = parameter as string;
            if (String.IsNullOrEmpty(parameterAsString))
                return Visibility.Collapsed;

            return value.ToString() == parameterAsString ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
