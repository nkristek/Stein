using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Stein.Converters
{
    public class BoolToVisibilityConverter
        : IValueConverter
    {
        public static BoolToVisibilityConverter Instance = new BoolToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as string)
            {
                case "Hidden": return (value is bool && (bool)value) ? Visibility.Hidden: Visibility.Collapsed;
                default: return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && ((Visibility)value == Visibility.Visible || ((Visibility)value == Visibility.Hidden));
        }
    }
}
