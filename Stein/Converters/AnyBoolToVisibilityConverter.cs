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
    public class AnyBoolToVisibilityConverter
        : IMultiValueConverter
    {
        public static AnyBoolToVisibilityConverter Instance = new AnyBoolToVisibilityConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as string)
            {
                case "Hidden": return values.Any(v => v is bool && (bool)v) ? Visibility.Hidden : Visibility.Collapsed;
                default: return values.Any(v => v is bool && (bool)v) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
