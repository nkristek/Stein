using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace nkristek.Ui.Converters
{
    /// <summary>
    /// Expects <see cref="IEnumerable{T}"/>.
    /// Returns true if it is not null or empty.
    /// </summary>
    public class IEnumerableNotNullOrEmptyToBoolConverter
        : IValueConverter
    {
        public static readonly IValueConverter Instance = new IEnumerableNotNullOrEmptyToBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is IEnumerable<object> enumerable && enumerable.Any();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
