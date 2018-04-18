using System;
using System.Globalization;
using System.Windows.Data;

namespace nkristek.Ui.Converters
{
    /// <summary>
    /// Expects a <see cref="bool"/>.
    /// Returns its opposite.
    /// </summary>
    public class BoolToInverseBoolConverter
        : IValueConverter
    {
        public static readonly IValueConverter Instance = new BoolToInverseBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && !b; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && !b;
        }
    }
}
