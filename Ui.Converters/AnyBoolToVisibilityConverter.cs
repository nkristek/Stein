using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace nkristek.Ui.Converters
{
    /// <summary>
    /// Expects a list of <see cref="bool"/>.
    /// Returns <see cref="Visibility.Visible"/> if any of them are true.
    /// </summary>
    public class AnyBoolToVisibilityConverter
        : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new AnyBoolToVisibilityConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => v is bool b && b))
                return Visibility.Visible;
            
            switch (parameter as string)
            {
                case "Hidden": return Visibility.Hidden;
                default: return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
