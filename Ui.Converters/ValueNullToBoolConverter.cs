﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace nkristek.Ui.Converters
{
    /// <summary>
    /// Expects <see cref="object"/>.
    /// Returns true if it is null.
    /// </summary>
    public class ValueNullToBoolConverter
        : IValueConverter
    {
        public static readonly IValueConverter Instance = new ValueNullToBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
