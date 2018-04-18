﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace nkristek.Ui.Converters
{
    /// <summary>
    /// Expects a list of <see cref="bool"/>.
    /// Returns true if all of them are false.
    /// </summary>
    public class AnyBoolToInverseBoolConverter
        : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new AnyBoolToInverseBoolConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return !values.Any(v => v is bool b && b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
